
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
                static IDeserialize<{typeFqn}> IDeserializeProvider<{typeFqn}>.DeserializeInstance
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
        ///   var serdeInfo = SerdeInfoProvider.GetInfo{T}();
        ///   var de = deserializer.ReadType(serdeInfo);
        ///   int index;
        ///   if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
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
                membersBuilder.AppendLine($"{i} => de.ReadValue<{m.ToDisplayString()}, {SerdeInfoGenerator.GetUnionProxyName(m)}>({i}),");
            }

            var src = new SourceBuilder($$"""
{{typeFqn}} IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{{typeFqn}}>();
    var de = deserializer.ReadType(serdeInfo);
    int index;
    if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
    {
        throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
    }
    {{typeFqn}} _l_result = index switch {
        {{membersBuilder}}
        _ => throw new InvalidOperationException($"Unexpected index: {index}")
    };
    if ((index = de.TryReadIndex(serdeInfo, out _)) != IDeserializeType.EndOfType)
    {
        throw Serde.DeserializeException.ExpectedEndOfType(index);
    }
    return _l_result;
}
""");
            return src;
        }

        /// <summary>
        /// Generates the method body for deserializing an enum.
        /// Code looks like:
        /// <code>
        /// static T IDeserialize&lt;T&gt;Deserialize(IDeserializer deserializer)
        /// {
        ///    var serdeInfo = SerdeInfoProvider.GetInfo{T}();
        ///    var de = deserializer.ReadType(serdeInfo);
        ///    int index;
        ///    if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
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
    var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{{typeFqn}}Proxy>();
    var de = deserializer.ReadType(serdeInfo);
    int index;
    if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
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
        ///     while ((index = typeDeserialize.TryReadIndex(serdeInfo)) != IDeserializeType.EndOfType)
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

    var {{typeInfoLocalName}} = global::Serde.SerdeInfoProvider.GetInfo<{{typeDeclContext.Name}}>();
    var typeDeserialize = deserializer.ReadType({{typeInfoLocalName}});
    int {{indexLocalName}};
    while (({{indexLocalName}} = typeDeserialize.TryReadIndex({{typeInfoLocalName}}, out {{errorNameOrDiscard}})) != IDeserializeType.EndOfType)
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
                    string wrapperName;
                    var memberType = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
                    if (Proxies.TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize, inProgress) is { } explicitWrap)
                    {
                        wrapperName = explicitWrap.ToString();
                    }
                    else if (ImplementsSerde(m.Type, m.Type, context, SerdeUsage.Deserialize))
                    {
                        wrapperName = memberType;
                    }
                    else if (Proxies.TryGetImplicitWrapper(m.Type, context, SerdeUsage.Deserialize, inProgress) is { Proxy: { } wrap })
                    {
                        wrapperName = wrap.ToString();
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
                        wrapperName = memberType;
                    }
                    var localName = GetLocalName(m);
                    localsBuilder.AppendLine($"{memberType} {localName} = default!;");
                    var readValueCall = GetReadValueCall(memberType, wrapperName);
                    casesBuilder.AppendLine($"""
                    case {fieldIndex}:
                        {localName} = typeDeserialize.{readValueCall}({indexLocalName});
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
                    : """
                    typeDeserialize.SkipValue();
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
                    case Serde.IDeserializeType.IndexNotFound:
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

                static string GetReadValueCall(
                    string memberType,
                    string wrapperName)
                {
                    return memberType switch {
                        "bool" => "ReadBool",
                        "char" => "ReadChar",
                        "byte" => "ReadByte",
                        "ushort" => "ReadU16",
                        "uint" => "ReadU32",
                        "ulong" => "ReadU64",
                        "sbyte" => "ReadSByte",
                        "short" => "ReadI16",
                        "int"   => "ReadI32",
                        "long"  => "ReadI64",
                        "float" => "ReadFloat",
                        "double" => "ReadDouble",
                        "decimal" => "ReadDecimal",
                        "string" => "ReadString",
                        _ => $"ReadValue<{memberType}, {wrapperName}>"
                    };
                }
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

        private static string GetLocalName(DataMemberSymbol m) => "_l_" + m.Name.ToLower();
    }
}