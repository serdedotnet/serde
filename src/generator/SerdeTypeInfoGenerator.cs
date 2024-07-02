
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde;

internal static class SerdeTypeInfoGenerator
{
    /// <summary>
    /// Generate the TypeInfo for the given type declaration.
    /// Looks like:
    /// <code>
    /// internal static class {typeName}SerdeTypeInfo
    /// {
    ///     internal static readonly TypeInfo TypeInfo = TypeInfo.Create([
    ///         ("{{fieldName}}", typeof({typeName}).GetField("{fieldName}")!),
    ///         ...
    ///     ]);
    /// }
    /// </code>
    /// </summary>
    public static void GenerateTypeInfo(
        BaseTypeDeclarationSyntax typeDecl,
        INamedTypeSymbol receiverType,
        GeneratorExecutionContext context)
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
        var newType = $$"""
internal static class {{typeName}}SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "{{typeName}}",
        Serde.TypeInfo.TypeKind.{{(receiverType.TypeKind == TypeKind.Enum ? "Enum" : "CustomType")}},
        new (string, System.Reflection.MemberInfo)[] {
{{string.Join("," + Environment.NewLine,
          fieldsAndProps.Select(x => $@"(""{x.GetFormattedName()}"", typeof({typeString}).Get{(x.Symbol.Kind == SymbolKind.Field ? "Field" : "Property")}(""{x.Name}"")!)"))}}
    });
}
""";

        newType = typeDeclContext.WrapNewType(newType);
        string fullTypeName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { $"{typeName}SerdeTypeInfo" }));

        context.AddSource(fullTypeName, newType);
    }
}