
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
        TypeDeclContext typeDeclContext,
        ITypeSymbol receiverType,
        GeneratorExecutionContext context,
        ImmutableList<ITypeSymbol> inProgress)
    {
        var typeName = typeDeclContext.Name;

        // Generate statements for the implementation
        var (implMembers, baseList) = usage switch
        {
            SerdeUsage.Serialize => SerializeImplRoslynGenerator.GenerateSerializeGenericImpl(context, receiverType, inProgress),
            SerdeUsage.Deserialize => DeserializeImplGenerator.GenerateDeserializeImpl(context, receiverType, inProgress),
            _ => throw ExceptionUtilities.Unreachable
        };

        var typeKind = typeDeclContext.Kind;
        MemberDeclarationSyntax newType;
        if (typeKind == SyntaxKind.EnumDeclaration)
        {
            var wrapperName = GetWrapperName(typeName);
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
        tree = tree.NormalizeWhitespace(eol: Environment.NewLine);

        context.AddSource(srcName,
            Environment.NewLine + "#nullable enable" + Environment.NewLine + tree.ToFullString());
    }

    /// <summary>
    /// Check to see if the <paramref name="targetType"/> implements ISerialize{<paramref
    /// name="argType"/>} or IDeserialize{<paramref name="argType"/>}, depending on the WrapUsage.
    /// </summary>
    internal static bool ImplementsSerde(ITypeSymbol targetType, ITypeSymbol argType, GeneratorExecutionContext context, SerdeUsage usage)
    {
        // Nullable types are not considered as implementing the Serde interfaces -- they use wrappers to map to the underlying
        if (argType.NullableAnnotation == NullableAnnotation.Annotated ||
            argType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            return false;
        }

        // Check if the type either has the GenerateSerialize attribute, or directly implements ISerialize
        // (If the type has the GenerateSerialize attribute then the generator will implement the interface)
        if (argType.TypeKind is not TypeKind.Enum && HasGenerateAttribute(argType, usage))
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

    private static TypeSyntax? TryGetEnumWrapper(ITypeSymbol type, SerdeUsage usage)
    {
        if (type.TypeKind is not TypeKind.Enum)
        {
            return null;
        }

        // Check for the generation attributes
        if (!HasGenerateAttribute(type, usage))
        {
            return null;
        }

        var wrapperName = GetWrapperName(type.Name);
        var containing = type.ContainingType?.ToDisplayString();
        if (containing is null && type.ContainingNamespace is { IsGlobalNamespace: false } ns)
        {
            containing = ns.ToDisplayString();
        }
        var wrapperFqn = containing is not null
             ? containing + "." + wrapperName
             : "global::" + wrapperName;

        return SyntaxFactory.ParseTypeName(wrapperFqn);
    }

    internal static string GetWrapperName(string typeName) => typeName + "Wrap";

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