
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Serde.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde
{
    partial class SerializeGenerator
    {
        public void GenerateWrapper(
            GeneratorExecutionContext context,
            AttributeSyntax attributeSyntax,
            TypeDeclarationSyntax typeDecl,
            SemanticModel model)
        {
            if (!typeDecl.Modifiers.Any(tok => tok.IsKind(SyntaxKind.PartialKeyword)))
            {
                // Type must be partial
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_TypeNotPartial,
                    typeDecl.Identifier.GetLocation(),
                    typeDecl.Identifier.ValueText));
                return;
            }

            var attrArgSyntax = attributeSyntax.ArgumentList?.Arguments[0];
            if (attrArgSyntax is null)
            {
                return;
            }
            var argValue = model.GetConstantValue(attrArgSyntax.Expression);
            if (argValue.Value is not string memberName)
            {
                return;
            }

            var members = model.LookupSymbols(typeDecl.SpanStart, name: memberName);
            if (members.Length != 1)
            {
                return;
            }
            var memberType = members[0] switch {
                IPropertySymbol p => p.Type,
                IFieldSymbol f => f.Type,
                _ => throw new InvalidOperationException()
            };

            BlockSyntax? block = null;
            var builtInName = GetBuiltInName(memberType);
            if (builtInName is not null)
            {
                block = Block(new SyntaxList<ExpressionStatementSyntax>(
                    ExpressionStatement(
                        InvocationExpression(
                            QualifiedName(IdentifierName("serializer"), IdentifierName("Serialize" + builtInName)),
                            ArgumentList(SeparatedList<ArgumentSyntax>(new[] { Argument(IdentifierName(memberName)) }))))));
            }

            var iserializeImpl = MethodDeclaration(
                attributeLists: default,
                modifiers: default,
                returnType: PredefinedType(Token(SyntaxKind.VoidKeyword)),
                explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(IdentifierName("ISerialize")),
                Identifier("Serialize"),
                typeParameterList: null,
                parameterList: ParameterList(SeparatedList(new[] { Parameter("ISerializer", "serializer") })),
                constraintClauses: default,
                body: block,
                expressionBody: null);

            var newDecl = typeDecl
                .WithAttributeLists(default)
                .WithBaseList(BaseList(SeparatedList(new BaseTypeSyntax[] {
                    SimpleBaseType(IdentifierName("ISerialize"))
                })))
                .WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PartialKeyword)))
                .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken))
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>(iserializeImpl));
            
            var tree = CompilationUnit(
                externs: default,
                usings: List(new[] { UsingDirective(IdentifierName("Serde")) }),
                attributeLists: default,
                members: List<MemberDeclarationSyntax>(new[] { newDecl }));
            tree = tree.NormalizeWhitespace(eol: Environment.NewLine);

            context.AddSource(
                typeDecl.Identifier.ToString(),
                Environment.NewLine + tree.ToFullString());
        }

        private static string? GetBuiltInName(ITypeSymbol type) => type.SpecialType switch 
        {
            SpecialType.System_String => "String",
            _ => null
        };
    }
}