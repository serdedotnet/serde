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

            var typeName = typeDecl.Identifier.ValueText;
            var semanticModel = syntaxReceiver.SemanticModel!;

            var typeDef = semanticModel.GetDeclaredSymbol(typeDecl)!;
            var fieldsAndProps = typeDef.GetMembers().Where(m => m is {
                    DeclaredAccessibility: Accessibility.Public,
                    Kind: SymbolKind.Field or SymbolKind.Property,
                }).ToList();

            // Generate statements for ISerialize.Serialize implementation
            var statements = GenerateISerializeStatements(typeName, fieldsAndProps);

            // Generate method `void ISerialize.Serialize<TSerializer, TSerializeType>(TSerializer serializer) { ... }`
            var newMethod = MethodDeclaration(attributeLists: default,
                modifiers: default,
                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(
                    QualifiedName(IdentifierName("Serde"), IdentifierName("ISerialize"))),
                identifier: Identifier("Serialize"),
                typeParameterList: TypeParameterList(SeparatedList(new[] {
                    "TSerializer", "TSerializeType"
                }.Select(s => TypeParameter(s)))),
                parameterList: ParameterList(SeparatedList(new[] { Parameter("TSerializer", "serializer") })),
                constraintClauses: default,
                body: Block(statements.ToArray()),
                semicolonToken: default
                );

            MemberDeclarationSyntax newType = typeDecl
                .WithAttributeLists(List<AttributeListSyntax>())
                .WithBaseList(BaseList(SeparatedList(new BaseTypeSyntax[] {
                    SimpleBaseType(QualifiedName(IdentifierName("Serde"), IdentifierName("ISerialize")))
                })))
                .WithMembers(List(new MemberDeclarationSyntax[] { newMethod }));

            // If the original type was in a namespace, put this decl in the same one
            if (typeDecl.Parent is NamespaceDeclarationSyntax ns)
            {
                newType = ns.WithMembers(List(new[] { newType }));
            }

            var tree = CompilationUnit(
                externs: default,
                usings: List(new[] { UsingDirective(IdentifierName("Serde")) }),
                attributeLists: default,
                members: List<MemberDeclarationSyntax>(new[] { newType }));
            tree = tree.NormalizeWhitespace(eol: Environment.NewLine);

            context.AddSource($"{typeName}.ISerialize.cs", Environment.NewLine + tree.ToFullString());
        }

        private List<StatementSyntax> GenerateISerializeStatements(
            string typeName,
            List<ISymbol> fieldsAndProps)
        {
            var statements = new List<StatementSyntax>(); 
            // `var type = serializer.SerializeType("TypeName", numFields)`
            statements.Add(LocalDeclarationStatement(VariableDeclaration(
                IdentifierName(Identifier("var")),
                SeparatedList(new[] {
                    VariableDeclarator(
                        Identifier("type"),
                        argumentList: null,
                        EqualsValueClause(InvocationExpression(
                            QualifiedName(IdentifierName("serializer"), IdentifierName("SerializeType")),
                            ArgumentList(SeparatedList(new [] {
                                Argument(StringLiteral(typeName)), Argument(NumericLiteral(fieldsAndProps.Count))
                            }))
                        ))
                    )
                })
            )));

            // Generate statements of the form `type.SerializeField("FieldName", FieldValue)`
            foreach (var m in fieldsAndProps)
            {
                var memberType = m switch
                {
                    IFieldSymbol { Type: var t } => t,
                    IPropertySymbol { Type: var t } => t,
                    _ => throw ExceptionUtilities.Unreachable
                };
                // If the target is a core type, we need to wrap it. Otherwise, just pass it through
                ExpressionSyntax fieldExpr = IdentifierName(m.Name);
                string? wrapperName = memberType.SpecialType switch 
                {
                    SpecialType.System_Byte => "ByteWrap",
                    SpecialType.System_UInt16 => "UInt16Wrap",
                    SpecialType.System_UInt32 => "UInt32Wrap",
                    SpecialType.System_UInt64 => "UInt64Wrap",
                    SpecialType.System_SByte => "SByteWrap",
                    SpecialType.System_Int16 => "Int16Wrap",
                    SpecialType.System_Int32 => "Int32Wrap",
                    SpecialType.System_Int64 => "Int64Wrap",
                    SpecialType.System_String => "StringWrap",
                    _ => null
                };
                if (wrapperName is not null)
                {
                    fieldExpr = ObjectCreationExpression(
                        IdentifierName(wrapperName),
                        ArgumentList(SeparatedList(new[] { Argument(fieldExpr) })),
                        initializer: null);
                }

                statements.Add(
                    ExpressionStatement(InvocationExpression(
                        // type.SerializeField
                        QualifiedName(IdentifierName("type"), IdentifierName("SerializeField")),
                        ArgumentList(SeparatedList(new ExpressionSyntax[] {
                            // "FieldName"
                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(m.Name)),
                            fieldExpr
                        }.Select(Argument))))));
            }

            statements.Add(ExpressionStatement(InvocationExpression(
                QualifiedName(IdentifierName("type"), IdentifierName("End")),
                ArgumentList()
            )));

            return statements;
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