using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Serde
{
    [Generator]
    public class SerdeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TypeDeclReceiver());
        }
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (TypeDeclReceiver)context.SyntaxContextReceiver!;
            var typeDecl = syntaxReceiver.TypeDeclarationSyntax;
            if (typeDecl is null)
            {
                return;
            }

            NamespaceDeclarationSyntax? namespaceDeclaration = null;
            for (SyntaxNode? current = typeDecl;
                 current is not null;
                 current = current.Parent)
            {
                if (current is NamespaceDeclarationSyntax ns)
                {
                    namespaceDeclaration = ns;
                    break;
                }
            }
            var typeName = typeDecl.Identifier.ValueText;
            var semanticModel = syntaxReceiver.SemanticModel!;

            var typeDef = semanticModel.GetDeclaredSymbol(typeDecl)!;
            var fieldsAndProps = typeDef.GetMembers().Where(m => m is {
                    DeclaredAccessibility: Accessibility.Public,
                    Kind: SymbolKind.Field or SymbolKind.Property,
                }).ToList();

            var statements = new List<StatementSyntax>(); 
            statements.Add(LocalDeclarationStatement(VariableDeclaration(
                IdentifierName(Identifier("var")),
                SeparatedList(new[] {
                    VariableDeclarator(
                        Identifier("type"),
                        argumentList: null,
                        EqualsValueClause(InvocationExpression(
                            QualifiedName(IdentifierName("serializer"), IdentifierName("SerializeStruct")),
                            ArgumentList(SeparatedList(new [] {
                                Argument(StringLiteral(typeName)), Argument(NumericLiteral(fieldsAndProps.Count))
                            }))
                        ))
                    )
                })
            )));

            statements.AddRange(fieldsAndProps.Select(m => 
                ExpressionStatement(InvocationExpression(
                    QualifiedName(IdentifierName("type"), IdentifierName("SerializeField")),
                    ArgumentList(SeparatedList(new ExpressionSyntax[] {
                        LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(m.Name)),
                        IdentifierName(m.Name)
                    }.Select(Argument)))))
            ));

            statements.Add(ExpressionStatement(InvocationExpression(
                QualifiedName(IdentifierName("type"), IdentifierName("End")),
                ArgumentList()
            )));

            var newMethod = MethodDeclaration(attributeLists: default,
                modifiers: TokenList(Token(SyntaxKind.PublicKeyword)),
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                explicitInterfaceSpecifier: default,
                identifier: Identifier("Serialize"),
                typeParameterList: TypeParameterList(SeparatedList(new[] {
                    "TSerializer", "TSerializeStruct"
                }.Select(s => TypeParameter(s)))),
                parameterList: ParameterList(SeparatedList(new[] { Parameter("TSerializer", "serializer") })),
                constraintClauses: List(new[] {
                    TypeParameterConstraintClause(
                        IdentifierName("TSerializer"),
                        SeparatedList(new TypeParameterConstraintSyntax[] {
                            TypeConstraint(QualifiedName(
                                IdentifierName("Serde"),
                                GenericName(
                                    Identifier("ISerializer"),
                                    TypeArgumentList(SeparatedList(new TypeSyntax[] { IdentifierName("TSerializeStruct") })))))
                        })
                    ),
                    TypeParameterConstraintClause(
                        IdentifierName("TSerializeStruct"),
                        SeparatedList(new TypeParameterConstraintSyntax[] {
                            TypeConstraint(QualifiedName(IdentifierName("Serde"), IdentifierName("ISerializeStruct")))
                        })
                    )
                }),
                body: Block(statements.ToArray()),
                semicolonToken: default
                );

            var newType = typeDecl
                .WithAttributeLists(List<AttributeListSyntax>())
                .WithBaseList(BaseList(SeparatedList(new BaseTypeSyntax[] {
                    SimpleBaseType(QualifiedName(IdentifierName("Serde"), IdentifierName("ISerialize")))
                })))
                .WithMembers(List(new MemberDeclarationSyntax[] { newMethod }));

            context.AddSource($"{typeName}.Serde.cs", newType.NormalizeWhitespace(eol: Environment.NewLine).ToFullString());
        }

        private static ParameterSyntax Parameter(string typeName, string paramName) => SyntaxFactory.Parameter(
            attributeLists: default,
            modifiers: default,
            type: IdentifierName(typeName),
            Identifier(paramName),
            default
        );

        private static LiteralExpressionSyntax NumericLiteral(int num)
            => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(num));

        private static LiteralExpressionSyntax StringLiteral(string text)
            => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(text));

        struct TypeDeclReceiver : ISyntaxContextReceiver
        {
            public TypeDeclarationSyntax? TypeDeclarationSyntax { get; private set; }
            public SemanticModel? SemanticModel { get; private set; }

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                var node = context.Node;
                if (node is TypeDeclarationSyntax typeDecl &&
                    (node.IsKind(SyntaxKind.ClassDeclaration)
                     || node.IsKind(SyntaxKind.StructDeclaration)
                     || node.IsKind(SyntaxKind.RecordDeclaration)))
                {
                    foreach (var attrLists in typeDecl.AttributeLists)
                    {
                        foreach (var attr in attrLists.Attributes)
                        {
                            var name = attr.Name;
                            while (name is QualifiedNameSyntax q)
                            {
                                name = q.Right;
                            }
                            if (name is IdentifierNameSyntax { Identifier: {
                                    ValueText: "GenerateSerde" or "GenerateSerdeAttribute"
                                } })
                            {
                                TypeDeclarationSyntax = typeDecl;
                                SemanticModel = context.SemanticModel;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}