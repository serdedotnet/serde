
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
        internal static SourceBuilder GenDeserialize(
            GeneratorExecutionContext context,
            ITypeSymbol receiverType,
            ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
        {
            var typeFqn = receiverType.ToDisplayString();
            TypeSyntax typeSyntax = ParseTypeName(typeFqn);

            if (receiverType.IsAbstract)
            {
                var memberDecl = GenUnionDeserializeMethod((INamedTypeSymbol)receiverType);
                return memberDecl;
            }

            // Generate members for IDeserialize.Deserialize implementation
            var members = receiverType.TypeKind == TypeKind.Enum
                ? GenerateEnumDeserializeMethod(receiverType, typeSyntax)
                : GenerateCustomDeserializeMethod(context, receiverType, typeSyntax, inProgress);
            return members;
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
                membersBuilder.AppendLine($"{i} => await de.ReadValue<{m.ToDisplayString()}, {SerdeInfoGenerator.GetUnionProxyName(m)}>(_l_serdeInfo, {i}),");
            }

            var src = new SourceBuilder($$"""
async global::System.Threading.Tasks.ValueTask<{{typeFqn}}> IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
    var de = deserializer.ReadType(_l_serdeInfo);
    int index;
    if ((index = await de.TryReadIndex(_l_serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
    {
        throw Serde.DeserializeException.UnknownMember(errorName!, _l_serdeInfo);
    }
    {{typeFqn}} _l_result = index switch {
        {{membersBuilder}}
        _ => throw new InvalidOperationException($"Unexpected index: {index}")
    };
    if ((index = await de.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
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
            var primName = Proxies.TryGetPrimitiveName(((INamedTypeSymbol)type).EnumUnderlyingType!);

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
async global::System.Threading.Tasks.ValueTask<{{typeFqn}}> IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
    var de = deserializer.ReadType(serdeInfo);
    int index = await de.TryReadIndex(serdeInfo, out var errorName);
    if (index == ITypeDeserializer.IndexNotFound)
    {
        throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
    }
    if (index == ITypeDeserializer.EndOfType)
    {
        // Assume we want to read the underlying value
        return ({{typeFqn}})(await de.Read{{primName}}(serdeInfo, index));
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
async global::System.Threading.Tasks.ValueTask<{{typeFqn}}> Serde.IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    {{locals}}
    {{assignedVarType}} {{AssignedVarName}} = 0;

    var {{typeInfoLocalName}} = global::Serde.SerdeInfoProvider.GetInfo(this);
    var typeDeserialize = deserializer.ReadType({{typeInfoLocalName}});
    int {{indexLocalName}};
    while (({{indexLocalName}} = await typeDeserialize.TryReadIndex({{typeInfoLocalName}}, out {{errorNameOrDiscard}})) != ITypeDeserializer.EndOfType)
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
                        {localName} = await typeDeserialize.{readValueCall}(_l_serdeInfo, {indexLocalName});
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
                    await typeDeserialize.SkipValue(_l_serdeInfo, {indexLocalName});
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

        private static string GetLocalName(DataMemberSymbol m) => "_l_" + m.Name.ToLower();
    }
}