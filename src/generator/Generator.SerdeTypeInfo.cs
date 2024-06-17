
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

        INamedTypeSymbol receiverType;
        ExpressionSyntax receiverExpr;
        string? wrapperName;
        string? wrappedName;
        // If the Through property is set, then we are implementing a wrapper type
        if (attributeData.NamedArguments is [ (nameof(GenerateSerialize.Through), { Value: string memberName }) ])
        {
            var members = model.LookupSymbols(typeDecl.SpanStart, typeSymbol, memberName);
            if (members.Length != 1)
            {
                // TODO: Error about bad lookup
                return;
            }
            receiverType = (INamedTypeSymbol)SymbolUtilities.GetSymbolType(members[0]);
            receiverExpr = IdentifierName(memberName);
            wrapperName = typeDecl.Identifier.ValueText;
            wrappedName = receiverType.ToDisplayString();
        }
        // Enums are also always wrapped, but the attribute is on the enum itself
        else if (typeDecl.IsKind(SyntaxKind.EnumDeclaration))
        {
            receiverType = typeSymbol;
            receiverExpr = IdentifierName("Value");
            wrappedName = typeDecl.Identifier.ValueText;
            wrapperName = GetWrapperName(wrappedName);
        }
        // Just a normal interface implementation
        else
        {
            wrapperName = null;
            wrappedName = null;
            if (!typeDecl.Modifiers.Any(tok => tok.IsKind(SyntaxKind.PartialKeyword)))
            {
                // Type must be partial
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_TypeNotPartial,
                    typeDecl.Identifier.GetLocation(),
                    typeDecl.Identifier.ValueText));
                return;
            }
            receiverType = typeSymbol;
            receiverExpr = ThisExpression();
        }

        GenerateTypeInfo(typeDecl, receiverType, context);
    }

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
        Serde.TypeInfo.TypeKind.CustomType,
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

    private static string GetWrapperName(string typeName) => typeName + "Wrap";
}