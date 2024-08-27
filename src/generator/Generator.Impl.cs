
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde;

[Flags]
internal enum SerdeUsage : byte
{
    Serialize = 0b01,
    Deserialize = 0b10,

    Both = Serialize | Deserialize,
}

partial class SerdeImplRoslynGenerator
{
    internal static void GenerateImpl(
        SerdeUsage usage,
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType,
        GeneratorExecutionContext context,
        ImmutableList<ITypeSymbol> inProgress)
    {
        var typeName = typeDeclContext.Name;

        // Generate statements for the implementation
        var (implMembers, baseList) = usage switch
        {
            SerdeUsage.Serialize => SerializeImplGen.GenSerialize(typeDeclContext, context, receiverType, inProgress),
            SerdeUsage.Deserialize => DeserializeImplGenerator.GenerateDeserializeImpl(typeDeclContext, context, receiverType, inProgress),
            _ => throw ExceptionUtilities.Unreachable
        };

        var typeKind = typeDeclContext.Kind;
        MemberDeclarationSyntax newType;
        if (typeKind == SyntaxKind.EnumDeclaration)
        {
            var wrapperName = Wrappers.GetWrapperName(typeName);
            newType = StructDeclaration(
                attributeLists: default,
                modifiers: TokenList(Token(SyntaxKind.PartialKeyword)),
                keyword: Token(SyntaxKind.StructKeyword),
                identifier: Identifier(wrapperName),
                typeParameterList: default,
                baseList: baseList,
                constraintClauses: default,
                openBraceToken: Token(SyntaxKind.OpenBraceToken),
                members: List(implMembers),
                closeBraceToken: Token(SyntaxKind.CloseBraceToken),
                semicolonToken: default);
            typeName = wrapperName;
        }
        else
        {
            newType = TypeDeclaration(
                typeKind,
                attributes: default,
                modifiers: TokenList(Token(SyntaxKind.PartialKeyword)),
                keyword: Token(typeKind switch
                {
                    SyntaxKind.ClassDeclaration => SyntaxKind.ClassKeyword,
                    SyntaxKind.StructDeclaration => SyntaxKind.StructKeyword,
                    SyntaxKind.RecordDeclaration
                    or SyntaxKind.RecordStructDeclaration => SyntaxKind.RecordKeyword,
                    _ => throw ExceptionUtilities.Unreachable
                }),
                identifier: Identifier(typeName),
                typeParameterList: typeDeclContext.TypeParameterList,
                baseList: baseList,
                constraintClauses: default,
                openBraceToken: Token(SyntaxKind.OpenBraceToken),
                members: List(implMembers),
                closeBraceToken: Token(SyntaxKind.CloseBraceToken),
                semicolonToken: default);
        }
        string fullTypeName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { typeName }));

        var srcName = fullTypeName + "." + usage.GetInterfaceName();

        newType = typeDeclContext.WrapNewType(newType);

        var tree = CompilationUnit(
            externs: default,
            usings: List(new[] { UsingDirective(IdentifierName("System")), UsingDirective(IdentifierName("Serde")) }),
            attributeLists: default,
            members: List<MemberDeclarationSyntax>(new[] { newType }));
        tree = tree.NormalizeWhitespace(eol: Utilities.NewLine);

        context.AddSource(srcName,
            Utilities.NewLine + "#nullable enable" + Utilities.NewLine + tree.ToFullString());
    }

    internal static (string FileName, string Body) MakePartialDecl(
        TypeDeclContext typeDeclContext,
        BaseListSyntax? baseList,
        string implMembers,
        string fileNameSuffix)
    {
        string typeName = typeDeclContext.Name;
        var typeKind = typeDeclContext.Kind;
        string declKeywords;
        (typeName, declKeywords) = typeKind == SyntaxKind.EnumDeclaration
            ? (Wrappers.GetWrapperName(typeName), "struct")
            : (typeName, TypeDeclContext.TypeKindToString(typeKind));
        var newType = $$"""
partial {{declKeywords}} {{typeName}}{{typeDeclContext.TypeParameterList}} : Serde.ISerdeInfoProvider
{
    {{implMembers}}
}
""";
        string fullTypeName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { typeName }));

        var srcName = fullTypeName + "." + fileNameSuffix;

        newType = typeDeclContext.WrapNewType(newType);

        return (srcName, Utilities.NewLine + "#nullable enable" + Utilities.NewLine + newType);
    }

    /// <summary>
    /// Check to see if the <paramref name="targetType"/> implements ISerialize{<paramref
    /// name="argType"/>} or IDeserialize{<paramref name="argType"/>}, depending on the WrapUsage.
    /// </summary>
    internal static bool ImplementsSerde(ITypeSymbol targetType, ITypeSymbol argType, GeneratorExecutionContext context, SerdeUsage usage)
    {
        // Nullable types are not considered as implementing the Serde interfaces -- they use wrappers to map to the underlying
        if (targetType.NullableAnnotation == NullableAnnotation.Annotated ||
            targetType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            return false;
        }

        // Check if the type either has the GenerateSerialize attribute, or directly implements ISerialize
        // (If the type has the GenerateSerialize attribute then the generator will implement the interface)
        if (targetType.TypeKind is not TypeKind.Enum && HasGenerateAttribute(targetType, usage))
        {
            return true;
        }

        var mdName = usage switch {
            SerdeUsage.Serialize => "Serde.ISerialize`1",
            SerdeUsage.Deserialize => "Serde.IDeserialize`1",
            _ => throw new ArgumentException("Invalid SerdeUsage", nameof(usage))
        };
        var serdeSymbol = context.Compilation.GetTypeByMetadataName(mdName)?.Construct(argType);

        if (serdeSymbol is not null && targetType.AllInterfaces.Contains(serdeSymbol, SymbolEqualityComparer.Default)
            || (targetType is ITypeParameterSymbol param && param.ConstraintTypes.Contains(serdeSymbol, SymbolEqualityComparer.Default)))
        {
            return true;
        }
        return false;
    }

    internal static bool HasGenerateAttribute(ITypeSymbol memberType, SerdeUsage usage)
    {
        var attributes = memberType.GetAttributes();
        foreach (var attr in attributes)
        {
            var attrClass = attr.AttributeClass;
            if (attrClass is null)
            {
                continue;
            }
            if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateSerde))
            {
                return true;
            }
            if (usage == SerdeUsage.Serialize &&
                WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateSerialize))
            {
                return true;
            }
            if (usage == SerdeUsage.Deserialize &&
                WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.GenerateDeserialize))
            {
                return true;
            }
        }
        return false;
    }
}