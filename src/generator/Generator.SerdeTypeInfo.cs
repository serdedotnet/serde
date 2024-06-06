
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde;

internal static class SerdeTypeInfoGenerator
{
    /// <summary>
    /// Generate the TypeInfo for the given type declaration.
    /// Looks like:
    /// <code>
    /// internal static class {typeName}SerdeTypeInfo
    /// {
    ///     internal static readonly TypeInfo TypeInfo = TypeInfo.Create&lt;{typeName}&gt;(nameof({typeName}), [
    ///         ("{{fieldName}}", typeof({typeName}).GetField("{fieldName}")!),
    ///         ...
    ///     ]);
    /// }
    /// </code>
    /// </summary>
    public static void GenerateTypeInfo(
        AttributeData attributeData,
        BaseTypeDeclarationSyntax typeDecl,
        SemanticModel model,
        GeneratorExecutionContext context)
    {
        var typeSymbol = model.GetDeclaredSymbol(typeDecl);
        if (typeSymbol is null)
        {
            return;
        }
        var receiverType = typeSymbol;

        var statements = new List<StatementSyntax>();
        var fieldsAndProps = SymbolUtilities.GetDataMembers(receiverType, SerdeUsage.Both);
        var typeDeclContext = new TypeDeclContext(typeDecl);
        var typeName = typeDeclContext.Name;
        var newType = $$"""
internal static class {{typeName}}SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
        {{string.Join("," + Environment.NewLine,
          fieldsAndProps.Select(x => $@"(""{x.GetFormattedName()}"", typeof({typeName}).Get{(x.Symbol.Kind == SymbolKind.Field ? "Field" : "Property")}(""{x.Name}"")!)"))}}
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