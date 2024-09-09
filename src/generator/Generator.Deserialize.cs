
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
    internal class DeserializeImplGenerator
    {
        internal static (MemberDeclarationSyntax[], BaseListSyntax) GenerateDeserializeImpl(
            TypeDeclContext typeDeclContext,
            GeneratorExecutionContext context,
            ITypeSymbol receiverType,
            ImmutableList<ITypeSymbol> inProgress)
        {
            var typeFqn = receiverType.ToDisplayString();
            TypeSyntax typeSyntax = ParseTypeName(typeFqn);

            // `Serde.IDeserialize<'typeName'>
            var interfaceSyntax = QualifiedName(IdentifierName("Serde"), GenericName(
                Identifier("IDeserialize"),
                TypeArgumentList(SeparatedList(new[] { typeSyntax }))
            ));

            // Generate members for ISerialize.Deserialize implementation
            MemberDeclarationSyntax[] members;
            if (receiverType.TypeKind == TypeKind.Enum)
            {
                var method = GenerateEnumDeserializeMethod(receiverType, typeSyntax);
                members = [ method ];
            }
            else
            {
                var method = GenerateCustomDeserializeMethod(typeDeclContext, context, receiverType, typeSyntax, inProgress);
                members = [ method ];
            }
            var baseList = BaseList(SeparatedList(new BaseTypeSyntax[] { SimpleBaseType(interfaceSyntax) }));
            return (members, baseList);
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
        private static MethodDeclarationSyntax GenerateEnumDeserializeMethod(
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
            var src = $$"""
static {{typeFqn}} IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<{{typeFqn}}Wrap>();
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
""";
            return (MethodDeclarationSyntax)ParseMemberDeclaration(src)!;
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
        private static MethodDeclarationSyntax GenerateCustomDeserializeMethod(
            TypeDeclContext typeDeclContext,
            GeneratorExecutionContext context,
            ITypeSymbol type,
            TypeSyntax typeSyntax,
            ImmutableList<ITypeSymbol> inProgress)
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
            string typeCreationExpr = GenerateTypeCreation(context, typeFqn, type, members, requiredMask);

            const string typeInfoLocalName = "_l_serdeInfo";
            const string indexLocalName = "_l_index_";
            const string IndexErrorName = "_l_errorName";

            var errorNameOrDiscard = SymbolUtilities.GetTypeOptions(type).DenyUnknownMembers
                ? $"var {IndexErrorName}"
                : "_";

            var methodText = $$"""
static {{typeFqn}} Serde.IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
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
""";
            return (MethodDeclarationSyntax)ParseMemberDeclaration(methodText)!;

            (string Cases, string Locals, string AssignedMask) InitCasesAndLocals()
            {
                var casesBuilder = new StringBuilder();
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
                    if (Wrappers.TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize, inProgress) is { } explicitWrap)
                    {
                        wrapperName = explicitWrap.ToString();
                    }
                    else if (ImplementsSerde(m.Type, m.Type, context, SerdeUsage.Deserialize))
                    {
                        wrapperName = memberType;
                    }
                    else if (Wrappers.TryGetImplicitWrapper(m.Type, context, SerdeUsage.Deserialize, inProgress) is { Wrapper: { } wrap })
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
                            "Serde.IDeserialize"));
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
                casesBuilder.AppendLine($"""
                    case Serde.IDeserializeType.IndexNotFound:
                        {unknownMemberBehavior}
                """);
                casesBuilder.AppendLine($"""
                    default:
                        throw new InvalidOperationException("Unexpected index: " + {indexLocalName});
                    """);
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
        /// must be a constructor signature as specified by the ConstructorSignature property
        /// in the SerdeTypeOptions.
        /// </summary>
        private static string GenerateTypeCreation(
            GeneratorExecutionContext context,
            string typeName,
            ITypeSymbol type,
            List<DataMemberSymbol> members,
            string assignedMask)
        {
            var targetSignature = SymbolUtilities.GetTypeOptions(type).ConstructorSignature;
            var targetTuple = targetSignature as INamedTypeSymbol;
            var ctors = type.GetMembers(".ctor");
            IMethodSymbol? targetCtor = null;
            IMethodSymbol? parameterLessCtor = null;
            foreach (var ctorSymbol in ctors)
            {
                if (ctorSymbol is IMethodSymbol ctorMethod)
                {
                    if (targetTuple is not null && ctorMethod.Parameters.Length == targetTuple.TupleElements.Length)
                    {
                        bool mismatch = false;
                        for(int i = 0; i < targetTuple.TupleElements.Length; i++)
                        {
                            var elem = targetTuple.TupleElements[i];
                            var param = ctorMethod.Parameters[i];
                            if (!elem.Type.Equals(param.Type, SymbolEqualityComparer.Default))
                            {
                                mismatch = true;
                                break;
                            }
                        }
                        if (!mismatch)
                        {
                            targetCtor = ctorMethod;
                            break;
                        }
                    }
                    if (ctorMethod is { Parameters.Length: 0 })
                    {
                        parameterLessCtor = ctorMethod;
                        if (targetSignature is null)
                        {
                            break;
                        }
                    }
                }
            }
            if (targetSignature is not null && targetCtor is null)
            {
                context.ReportDiagnostic(CreateDiagnostic(DiagId.ERR_CantFindConstructorSignature, type.Locations[0]));
                return "";
            }

            var assignmentMembers = new List<DataMemberSymbol>(members);
            var assignments = new StringBuilder();
            var parameters = new StringBuilder();
            if (targetCtor is not null)
            {
                foreach (var p in targetCtor.Parameters)
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

            foreach (var m in assignmentMembers)
            {
                if (m.SkipDeserialize)
                {
                    continue;
                }
                assignments.AppendLine($"{m.Name} = {GetLocalName(m)},");
            }
            var mask = new string('1', members.Count);
            return $$"""
    if (({{AssignedVarName}} & {{assignedMask}}) != {{assignedMask}})
    {
        throw Serde.DeserializeException.UnassignedMember();
    }
    var newType = new {{typeName}}({{parameters}}) {
        {{assignments}}
    };
""";
        }

        private static string GetLocalName(DataMemberSymbol m) => "_l_" + m.Name.ToLower();
    }
}