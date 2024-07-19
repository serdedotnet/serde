
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde;

internal static class SerdeInfoGenerator
{
    /// <summary>
    /// Generate the TypeInfo for the given type declaration.
    /// Looks like:
    /// <code>
    /// partial {typeKind} {typeName}
    /// {
    ///     internal static readonly SerdeInfo TypeInfo = TypeInfo.Create([
    ///         ("{{fieldName}}", typeof({typeName}).GetField("{fieldName}")!),
    ///         ...
    ///     ]);
    /// }
    /// </code>
    /// </summary>
    public static void GenerateSerdeInfo(
        BaseTypeDeclarationSyntax typeDecl,
        INamedTypeSymbol receiverType,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<ITypeSymbol> inProgress)
    {
        var statements = new List<StatementSyntax>();
        var fieldsAndProps = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Both);
        var typeDeclContext = new TypeDeclContext(typeDecl);
        var typeName = receiverType.Name;
        var typeString = receiverType.ToDisplayString();
        if (typeString.IndexOf('<') is var index && index != -1)
        {
            typeString = typeString[..index];
            typeString = typeString + "<" + new string(',', receiverType.TypeParameters.Length - 1) + ">";
        }
        var body = $$"""
static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "{{typeName}}",
        Serde.SerdeInfo.TypeKind.{{(receiverType.TypeKind == TypeKind.Enum ? "Enum" : "CustomType")}},
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
{{string.Join("," + Environment.NewLine, fieldsAndProps.SelectNotNull(GetMemberEntry))}}
    });
""";
        var (fileName, newType) = SerdeImplRoslynGenerator.MakePartialDecl(
            typeDeclContext,
            baseList: null,
            body,
            "ISerdeInfoProvider");

        context.AddSource(fileName, newType);

        // Returns a string that represents a single member in the custom SerdeInfo member list.
        string? GetMemberEntry(DataMemberSymbol m)
        {
            var fieldName = m.GetFormattedName();
            var getAccessor = m.Symbol.Kind == SymbolKind.Field ? "GetField" : "GetProperty";
            string wrapperName;
            if (Wrappers.TryGetExplicitWrapper(m, context, usage, inProgress) is { } explicitWrap)
            {
                wrapperName = explicitWrap.ToString();
            }
            else if (SerdeImplRoslynGenerator.ImplementsSerde(m.Type, m.Type, context, usage))
            {
                wrapperName = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
            }
            else if (Wrappers.TryGetImplicitWrapper(m.Type, context, usage, inProgress) is { Wrapper: { } wrap })
            {
                wrapperName = wrap.ToString();
            }
            else
            {
                // This is an error, but it should have already been produced by the serialization
                // or deserialization generator
                return null;
            }
            var fieldInfo = $"global::Serde.SerdeInfoProvider.GetInfo<{wrapperName}>()";

            return $"""("{fieldName}", {fieldInfo}, typeof({typeString}).{getAccessor}("{m.Name}")!)""";
        }
    }
}