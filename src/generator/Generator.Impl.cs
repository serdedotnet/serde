
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde
{
    internal enum SerdeUsage : byte
    {
        Serialize = 0b01,
        Deserialize = 0b10,
    }

    partial class Generator
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
            if (!_registry.TryAdd(context.Compilation, srcName))
            {
                return;
            }

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
                typeParameterList: null,
                baseList: baseList,
                constraintClauses: default,
                openBraceToken: Token(SyntaxKind.OpenBraceToken),
                members: List(implMembers),
                closeBraceToken: Token(SyntaxKind.CloseBraceToken),
                semicolonToken: default);

            newType = typeDeclContext.WrapNewType(newType);

            var tree = CompilationUnit(
                externs: default,
                usings: List(new[] { UsingDirective(IdentifierName("Serde")) }),
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

        public static string GetName(this SerdeUsage usage) => usage switch
        {
            SerdeUsage.Serialize => "SerializeImpl",
            SerdeUsage.Deserialize => "DeserializeImpl",
            _ => throw ExceptionUtilities.Unreachable
        };
    }
}