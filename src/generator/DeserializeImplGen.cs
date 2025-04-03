
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.SerdeImplRoslynGenerator;

namespace Serde
{
    internal class DeserializeImplGen
    {
        internal static (SourceBuilder, string BaseList) GenDeserialize(
            TypeDeclContext typeDeclContext,
            GeneratorExecutionContext context,
            ITypeSymbol receiverType,
            ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
        {
            var typeFqn = receiverType.ToDisplayString();
            TypeSyntax typeSyntax = ParseTypeName(typeFqn);

            if (receiverType.IsAbstract)
            {
                var memberDecl = GenUnionDeserializeMethod((INamedTypeSymbol)receiverType);
                return (memberDecl, $": global::Serde.IDeserialize<{typeFqn}>");
            }

            var fieldsAndProps = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Serialize);
            var inlined = fieldsAndProps.Where(m => m.Inline).ToList();
            if (inlined.Count > 1)
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_MultipleInline,
                    typeDeclContext.TypeDecl.GetLocation()
                ));
                return (new(""), $": global::Serde.IDeserialize<{typeFqn}>");
            }
            if (inlined.Count == 1)
            {
                if (fieldsAndProps.Count > 1)
                {
                    context.ReportDiagnostic(CreateDiagnostic(
                        DiagId.ERR_InlineWithOtherMembers,
                        typeDeclContext.TypeDecl.GetLocation()
                    ));
                    return (new(""), $" : Serde.IDeserialize<{typeFqn}>");
                }
                return GenInline(
                    inlined[0],
                    receiverType,
                    context,
                    inProgress
                );
            }

            // Generate members for IDeserialize.Deserialize implementation
            var members = new SourceBuilder();
            List<BaseTypeSyntax> bases = [
                // `Serde.IDeserialize<'typeName'>
                SimpleBaseType(QualifiedName(IdentifierName("Serde"), GenericName(
                    Identifier("IDeserialize"),
                    TypeArgumentList(SeparatedList(new[] { typeSyntax }))
                ))),
            ];
            if (receiverType.TypeKind == TypeKind.Enum)
            {
                // `Serde.IDeserializeProvider<'typeName'>. Enums generate a proxy
                bases.Add(SimpleBaseType(ParseTypeName($"Serde.IDeserializeProvider<{typeFqn}>")));

                var deserialize = GenerateEnumDeserializeMethod(receiverType, typeSyntax);
                members.AppendLine(deserialize);

                members.AppendLine($"""
                static IDeserialize<{typeFqn}> IDeserializeProvider<{typeFqn}>.Instance
                    => {typeFqn}Proxy.Instance;
                """);
            }
            else
            {
                var method = GenerateCustomDeserializeMethod(typeDeclContext, context, receiverType, typeSyntax, inProgress);
                members.AppendLine(method);
            }
            var baseList = BaseList(SeparatedList(bases));
            return (members, baseList.ToFullString());
        }

        /// <summary>
        /// Generates the method body for deserializing a union.
        /// Code looks like:
        /// <code>
        /// static T IDeserialize&lt;T&gt;Deserialize(IDeserializer deserializer)
        /// {
        ///   var serdeInfo = GetInfo(this);
        ///   var de = deserializer.ReadType(serdeInfo);
        ///   int index;
        ///   if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
        ///   {
        ///    throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        ///   }
        ///   return index switch
        ///   {
        ///     {index} => deserializer.Deserialize({union member}),
        ///     ...
        ///     _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")
        ///   };
        /// }
        /// </code>
        /// </summary>
        private static SourceBuilder GenUnionDeserializeMethod(INamedTypeSymbol type)
        {
            Debug.Assert(type.IsAbstract);

            var members = SymbolUtilities.GetDUTypeMembers(type);
            var typeFqn = type.ToDisplayString();
            var assignedVarType = members.Length switch {
                (<= 8) => "byte",
                (<= 16) => "ushort",
                (<= 32) => "uint",
                (<= 64) => "ulong",
                _ => throw new InvalidOperationException("Too many members in type")
            };
            var membersBuilder = new StringBuilder();
            for (int i = 0; i < members.Length; i++)
            {
                var m = members[i];
                membersBuilder.AppendLine($"{i} => de.ReadValue<{m.ToDisplayString()}, {SerdeInfoGenerator.GetUnionProxyName(m)}>(_l_serdeInfo, {i}),");
            }

            var src = new SourceBuilder($$"""
{{typeFqn}} IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
    var de = deserializer.ReadType(_l_serdeInfo);
    int index;
    if ((index = de.TryReadIndex(_l_serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
    {
        throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
    }
    {{typeFqn}} _l_result = index switch {
        {{membersBuilder}}
        _ => throw new InvalidOperationException($"Unexpected index: {index}")
    };
    if ((index = de.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
    {
        throw Serde.DeserializeException.ExpectedEndOfType(index);
    }
    return _l_result;
}
""");
            return src;
        }

        private static (SourceBuilder Members, string BaseList) GenInline(
            DataMemberSymbol m,
            ITypeSymbol receiverType,
            GeneratorExecutionContext context,
            ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress
        )
        {
            var wrapper = GetProxyName(m, context, inProgress);
            if (wrapper is null)
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_DoesntImplementInterface,
                    m.Locations[0],
                    m.Symbol,
                    m.Type,
                    "Serde.IDeserializeProvider<T>"));
            }
            var typeName = m.Type.ToDisplayString();
            var receiverName = receiverType.ToDisplayString();
            var infoText = $$"""
{{receiverName}} global::Serde.IDeserialize<{{receiverName}}>.Deserialize(global::Serde.IDeserializer deserializer)
{
    var deObj = global::Serde.DeserializeProvider.GetDeserialize<{{typeName}}, {{wrapper}}>();
    return new(deObj.Deserialize(deserializer));
}

""";
            return (new(infoText), $" : global::Serde.IDeserialize<{receiverName}>");
        }

        /// <summary>
        /// Generates the method body for deserializing an enum.
        /// Code looks like:
        /// <code>
        /// static T IDeserialize&lt;T&gt;Deserialize(IDeserializer deserializer)
        /// {
        ///    var serdeInfo = GetInfo(this);
        ///    var de = deserializer.ReadType(serdeInfo);
        ///    int index;
        ///    if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
        ///    {
        ///      throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        ///    }
        ///    return index switch
        ///    {
        ///      {index} =&gt; {enum member},
        ///      ...
        ///      _ =&gt; throw new InvalidDeserializeValueException($"Unexpected index: {index}")
        ///    };
        /// }
        /// </code>
        /// </summary>
        private static SourceBuilder GenerateEnumDeserializeMethod(
            ITypeSymbol type,
            TypeSyntax typeSyntax)
        {
            Debug.Assert(type.TypeKind == TypeKind.Enum);

            var members = SymbolUtilities.GetDataMembers(type, SerdeUsage.Both);
            var typeFqn = typeSyntax.ToString();
            var assignedVarType = members.Count switch {
                <= 8 => "byte",
                <= 16 => "ushort",
                <= 32 => "uint",
                <= 64 => "ulong",
                _ => throw new InvalidOperationException("Too many members in type")
            };
            var src = new SourceBuilder($$"""
{{typeFqn}} IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
    var de = deserializer.ReadType(serdeInfo);
    int index;
    if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
    {
        throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
    }
    return index switch {
        {{string.Join("," + Utilities.NewLine, members
            .Select((m, i) => $"{i} => {typeSyntax}.{m.Name}")) }},
        _ => throw new InvalidOperationException($"Unexpected index: {index}")
    };
}
""");
            return src;
        }

        /// <summary>
        /// Generates
        /// <code>
        /// T IDeserialize&lt;T&gt;.Deserialize(IDeserializer deserializer)
        /// {
        ///     var _local1 = default!;
        ///     ...
        ///     var _localN = default!;
        ///
        ///     var serdeInfo = {typeName}SerdeInfo.Instance;
        ///     var typDeserializer = deserializer.DeserializeType(serdeInfo);
        ///     int index;
        ///     while ((index = typeDeserialize.TryReadIndex(serdeInfo)) != ITypeDeserializer.EndOfType)
        ///     {
        ///         switch (index)
        ///         {
        ///         }
        ///     }
        /// }
        /// </code>
        /// </summary>
        private static SourceBuilder GenerateCustomDeserializeMethod(
            TypeDeclContext typeDeclContext,
            GeneratorExecutionContext context,
            ITypeSymbol type,
            TypeSyntax typeSyntax,
            ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
        {
            Debug.Assert(type.TypeKind != TypeKind.Enum);

            var members = SymbolUtilities.GetDataMembers(type, SerdeUsage.Both);
            var typeFqn = typeSyntax.ToString();
            var assignedVarType = members.Count switch {
                <= 8 => "byte",
                <= 16 => "ushort",
                <= 32 => "uint",
                <= 64 => "ulong",
                _ => throw new InvalidOperationException("Too many members in type")
            };
            var (cases, locals, requiredMask) = InitCasesAndLocals();
            var typeCreationExpr = GenerateTypeCreation(context, typeFqn, type, members, requiredMask);

            const string typeInfoLocalName = "_l_serdeInfo";
            const string indexLocalName = "_l_index_";
            const string IndexErrorName = "_l_errorName";

            var errorNameOrDiscard = SymbolUtilities.GetTypeOptions(type).DenyUnknownMembers
                ? $"var {IndexErrorName}"
                : "_";

            var methodText = new SourceBuilder($$"""
{{typeFqn}} Serde.IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    {{locals}}
    {{assignedVarType}} {{AssignedVarName}} = 0;

    var {{typeInfoLocalName}} = global::Serde.SerdeInfoProvider.GetInfo(this);
    var typeDeserialize = deserializer.ReadType({{typeInfoLocalName}});
    int {{indexLocalName}};
    while (({{indexLocalName}} = typeDeserialize.TryReadIndex({{typeInfoLocalName}}, out {{errorNameOrDiscard}})) != ITypeDeserializer.EndOfType)
    {
        switch ({{indexLocalName}})
        {
            {{cases}}
        }
    }
    {{typeCreationExpr}}
    return newType;
}
""");
            return methodText;

            (string Cases, string Locals, string AssignedMask) InitCasesAndLocals()
            {
                var casesBuilder = new SourceBuilder();
                var localsBuilder = new StringBuilder();
                long assignedMaskValue = 0;
                var skippedIndices = new List<int>();
                for (int fieldIndex = 0; fieldIndex < members.Count; fieldIndex++)
                {
                    if (members[fieldIndex].SkipDeserialize)
                    {
                        skippedIndices.Add(fieldIndex);
                        continue;
                    }

                    var m = members[fieldIndex];
                    var memberType = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
                    string readMethodName = m.Type.IsReferenceType
                        ? "ReadValue"
                        : "ReadBoxedValue";
                    string readValueCall;
                    if (Proxies.TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize, inProgress) is { } explicitWrap)
                    {
                        readValueCall = $"{readMethodName}<{memberType}, {explicitWrap}>";
                    }
                    else if (ImplementsSerde(m.Type, m.Type, context, SerdeUsage.Deserialize))
                    {
                        readValueCall = $"{readMethodName}<{memberType}, {memberType}>";
                    }
                    else if (Proxies.TryGetPrimitiveName(m.Type) is { } primitiveName)
                    {
                        readValueCall = $"Read{primitiveName}";
                    }
                    else if (Proxies.TryGetImplicitWrapper(m.Type, context, SerdeUsage.Deserialize, inProgress) is { Proxy: { } wrap })
                    {
                        readValueCall = $"{readMethodName}<{memberType}, {wrap}>";
                    }
                    else
                    {
                        // No built-in handling and doesn't implement IDeserialize, error
                        context.ReportDiagnostic(CreateDiagnostic(
                            DiagId.ERR_DoesntImplementInterface,
                            m.Locations[0],
                            m.Symbol,
                            memberType,
                            "Serde.IDeserializeProvider<T>"));
                        readValueCall = $"ReadValue<{memberType}, {memberType}>";
                    }
                    var localName = GetLocalName(m);
                    localsBuilder.AppendLine($"{memberType} {localName} = default!;");
                    casesBuilder.AppendLine($"""
                    case {fieldIndex}:
                        {localName} = typeDeserialize.{readValueCall}(_l_serdeInfo, {indexLocalName});
                        {AssignedVarName} |= (({assignedVarType})1) << {fieldIndex};
                        break;
                    """);

                    // Require that the member is assigned if m.ThrowIfMissing is set, or if it is not nullable
                    // and ThrowIfMissing is unset
                    if (m.ThrowIfMissing == true || (!m.IsNullable && m.ThrowIfMissing == null))
                    {
                        assignedMaskValue |= 1L << fieldIndex;
                    }
                }
                var unknownMemberBehavior = SymbolUtilities.GetTypeOptions(type).DenyUnknownMembers
                    ? $"""
                    throw Serde.DeserializeException.UnknownMember(_l_errorName!, {typeInfoLocalName});
                    """
                    : $"""
                    typeDeserialize.SkipValue(_l_serdeInfo, {indexLocalName});
                    break;
                    """;
                foreach (var i in skippedIndices)
                {
                    casesBuilder.AppendLine($"""
                    case {i}:
                    """);
                }
                casesBuilder.AppendLine(
                    $"""
                    case Serde.ITypeDeserializer.IndexNotFound:
                        {unknownMemberBehavior}
                    """
                );
                casesBuilder.Append(
                    $"""
                    default:
                        throw new InvalidOperationException("Unexpected index: " + {indexLocalName});
                    """
                );
                return (casesBuilder.ToString(),
                        localsBuilder.ToString(),
                        "0b" + Convert.ToString(assignedMaskValue, 2));
            }
        }

        private const string AssignedVarName = "_r_assignedValid";

        /// <summary>
        /// If the type has a parameterless constructor then we will use that and just set
        /// each member in the initializer. If there is no parameterlss constructor, there
        /// must be a primary constructor.
        /// </summary>
        private static SourceBuilder GenerateTypeCreation(
            GeneratorExecutionContext context,
            string typeName,
            ITypeSymbol type,
            List<DataMemberSymbol> members,
            string assignedMask)
        {
            IMethodSymbol? primaryCtor = null;
            IMethodSymbol? parameterlessCtor = null;
            if (type is INamedTypeSymbol named)
            {
                foreach (var ctor in named.InstanceConstructors)
                {
                    foreach (var syntaxRef in ctor.DeclaringSyntaxReferences)
                    {
                        var syntax = syntaxRef.GetSyntax();
                        if (syntax is TypeDeclarationSyntax)
                        {
                            primaryCtor = ctor;
                        }
                    }
                    if (ctor.Parameters.Length == 0)
                    {
                        parameterlessCtor = ctor;
                        break;
                    }
                }
            }

            if (parameterlessCtor is null && primaryCtor is null)
            {
                context.ReportDiagnostic(CreateDiagnostic(DiagId.ERR_MissingPrimaryCtor, type.Locations[0]));
                return new SourceBuilder($"var newType = new {typeName}();");
            }

            var assignmentMembers = new List<DataMemberSymbol>(members);
            var parameters = new StringBuilder();
            if (primaryCtor is not null)
            {
                foreach (var p in primaryCtor.Parameters)
                {
                    var index = assignmentMembers.FindIndex(m => m.Name == p.Name);
                    if (parameters.Length != 0)
                    {
                        parameters.Append(", ");
                    }
                    parameters.Append(GetLocalName(assignmentMembers[index]));
                    assignmentMembers.RemoveAt(index);
                }
            }

            var typeCreation = new SourceBuilder(
                $$"""
                if (({{AssignedVarName}} & {{assignedMask}}) != {{assignedMask}})
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new {{typeName}}({{parameters}}) {

                """
            );
            typeCreation.Indent();
            foreach (var m in assignmentMembers)
            {
                if (m.SkipDeserialize)
                {
                    continue;
                }
                typeCreation.AppendLine($"{m.Name} = {GetLocalName(m)},");
            }
            typeCreation.Dedent();
            typeCreation.AppendLine("};");
            return typeCreation;
        }

        private static string? GetProxyName(
            DataMemberSymbol m,
            GeneratorExecutionContext context,
            ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress
        )
        {
            if (Proxies.TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize, inProgress) is { } explicitWrap)
            {
                return explicitWrap;
            }
            else if (ImplementsSerde(m.Type, m.Type, context, SerdeUsage.Deserialize))
            {
                return m.Type.ToDisplayString();
            }
            else if (Proxies.TryGetImplicitWrapper(m.Type, context, SerdeUsage.Deserialize, inProgress) is { Proxy: { } wrap })
            {
                return wrap;
            }
            else
            {
                // No built-in handling and doesn't implement IDeserialize, error
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_DoesntImplementInterface,
                    m.Locations[0],
                    m.Symbol,
                    m.Type,
                    "Serde.IDeserializeProvider<T>"));
                return null;
            }
        }

        private static string GetLocalName(DataMemberSymbol m) => "_l_" + m.Name.ToLower();
    }
}