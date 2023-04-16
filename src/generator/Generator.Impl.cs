
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

internal sealed class GeneratorExecutionContext
{
    private bool _frozen = false;
    private readonly List<Diagnostic> _diagnostics = new();
    private readonly SortedSet<(string FileName, string Content)> _sources = new();

    public Compilation Compilation { get; }

    public GeneratorExecutionContext(GeneratorAttributeSyntaxContext context)
    {
        Compilation = context.SemanticModel.Compilation;
    }

    public GenerationOutput GetOutput()
    {
        _frozen = true;
        return new GenerationOutput(_diagnostics, _sources);
    }

    public void ReportDiagnostic(Diagnostic diagnostic)
    {
        if (_frozen)
            throw new InvalidOperationException("Cannot add diagnostics after GetDiagnostics() has been called.");
        _diagnostics.Add(diagnostic);
    }

    internal void AddSource(string fileName, string content)
    {
        if (_frozen)
            throw new InvalidOperationException("Cannot add sources after GetSources() has been called.");
        _sources.Add((fileName, content));
    }
}

partial class SerdeImplRoslynGenerator
{
    internal static void GenerateImpl(
        SerdeUsage usage,
        TypeDeclarationSyntax typeDecl,
        SemanticModel semanticModel,
        GeneratorExecutionContext context)
    {
        var receiverType = semanticModel.GetDeclaredSymbol(typeDecl);
        if (receiverType is null)
        {
            return;
        }
        if (!typeDecl.Modifiers.Any(tok => tok.IsKind(SyntaxKind.PartialKeyword)))
        {
            // Type must be partial
            context.ReportDiagnostic(CreateDiagnostic(
                DiagId.ERR_TypeNotPartial,
                typeDecl.Identifier.GetLocation(),
                typeDecl.Identifier.ValueText));
            return;
        }

        var receiverExpr = ThisExpression();
        GenerateImpl(usage, new TypeDeclContext(typeDecl), receiverType, receiverExpr, context);
    }

    private static void GenerateImpl(
        SerdeUsage usage,
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType,
        ExpressionSyntax receiverExpr,
        GeneratorExecutionContext context)
    {
        var typeName = typeDeclContext.Name;
        string fullTypeName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { typeName }));

        string srcName = fullTypeName + "." + usage.GetInterfaceName();

        // Generate statements for the implementation
        var (implMembers, baseList) = usage switch
        {
            SerdeUsage.Serialize => GenerateSerializeImpl(context, receiverType, receiverExpr),
            SerdeUsage.Deserialize => GenerateDeserializeImpl(context, receiverType, receiverExpr),
            _ => throw ExceptionUtilities.Unreachable
        };

        var typeKind = typeDeclContext.Kind;
        MemberDeclarationSyntax newType = TypeDeclaration(
            typeKind,
            attributes: default,
            modifiers: TokenList(Token(SyntaxKind.PartialKeyword)),
            keyword: Token(typeKind switch {
                SyntaxKind.ClassDeclaration => SyntaxKind.ClassKeyword,
                SyntaxKind.StructDeclaration => SyntaxKind.StructKeyword,
                SyntaxKind.RecordDeclaration
                or SyntaxKind.RecordStructDeclaration=> SyntaxKind.RecordKeyword,
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

        newType = typeDeclContext.WrapNewType(newType);

        var tree = CompilationUnit(
            externs: default,
            usings: List(new[] { UsingDirective(IdentifierName("System")), UsingDirective(IdentifierName("Serde")) }),
            attributeLists: default,
            members: List<MemberDeclarationSyntax>(new[] { newType }));
        tree = tree.NormalizeWhitespace(eol: Environment.NewLine);

        context.AddSource(srcName,
            Environment.NewLine + "#nullable enable" + Environment.NewLine + tree.ToFullString());
    }
}

internal static class WrapUsageExtensions
{
    public static string GetInterfaceName(this SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "ISerialize",
        SerdeUsage.Deserialize => "IDeserialize",
        _ => throw ExceptionUtilities.Unreachable
    };

    public static string GetImplName(this SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "SerializeImpl",
        SerdeUsage.Deserialize => "DeserializeImpl",
        _ => throw ExceptionUtilities.Unreachable
    };
}