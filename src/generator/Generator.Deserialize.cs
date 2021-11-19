
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde
{
    partial class Generator
    {
        private const string GeneratedVisitorName = "SerdeVisitor";

        private void GenerateDeserialize(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel semanticModel)
        {
            var receiverType = semanticModel.GetDeclaredSymbol(typeDecl);
            if (receiverType is null)
            {
                return;
            }
            var receiverExpr = ThisExpression();
            GenerateDeserialize(context, typeDecl, semanticModel, receiverType, receiverExpr);
        }

        private void GenerateDeserialize(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel model,
            ITypeSymbol receiverType,
            ExpressionSyntax receiverExpr)
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

            TypeSyntax typeSyntax = ParseTypeName(receiverType.ToDisplayString());

            // Generate statements for ISerialize.Deserialize implementation
            var statements = GenerateDeserializeBody(receiverType);

            if (statements is not null)
            {
                // `Serde.IDeserialize<'typeName'>
                var interfaceSyntax = QualifiedName(IdentifierName("Serde"), GenericName(
                    Identifier("IDeserialize"),
                    TypeArgumentList(SeparatedList(new[] { typeSyntax }))
                ));

                // Generate method `void ISerialize.Deserialize(IDeserializer deserializer) { ... }`
                var newMethod = MethodDeclaration(
                    attributeLists: default,
                    modifiers: TokenList(Token(SyntaxKind.StaticKeyword)),
                    typeSyntax,
                    explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(interfaceSyntax),
                    identifier: Identifier("Deserialize"),
                    typeParameterList: TypeParameterList(SeparatedList(new[] {
                        TypeParameter("D"),
                        })),
                    parameterList: ParameterList(SeparatedList(new[] { Parameter("D", "deserializer", byRef: true) })),
                    constraintClauses: default,
                    body: Block(statements.ToArray()),
                    expressionBody: null
                    );

                var visitorType = GenerateVisitor(receiverType, typeSyntax, context);

                MemberDeclarationSyntax newType = typeDecl
                    .WithModifiers(TokenList(Token(SyntaxKind.PartialKeyword)))
                    .WithAttributeLists(List<AttributeListSyntax>())
                    .WithBaseList(BaseList(SeparatedList(new BaseTypeSyntax[] { SimpleBaseType(interfaceSyntax) })))
                    .WithSemicolonToken(default)
                    .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                    .WithMembers(List(new MemberDeclarationSyntax[] { newMethod, visitorType }))
                    .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken));

                // If the original type was a record, remove the parameter list
                if (newType is RecordDeclarationSyntax record)
                {
                    newType = record.WithParameterList(null);
                }

                // If the original type was in a namespace, put this decl in the same one
                switch (typeDecl.Parent)
                {
                    case NamespaceDeclarationSyntax ns:
                        newType = ns.WithMembers(List(new[] { newType }));
                        break;
                    case TypeDeclarationSyntax parentType:
                        newType = parentType.WithMembers(List(new[] { newType}));
                        break;
                }

                var tree = CompilationUnit(
                    externs: default,
                    usings: List(new[] { UsingDirective(IdentifierName("Serde")) }),
                    attributeLists: default,
                    members: List<MemberDeclarationSyntax>(new[] { newType }));
                tree = tree.NormalizeWhitespace(eol: Environment.NewLine);

                string fullTypeName = typeDecl.Identifier.ValueText;
                for (var current = typeDecl.Parent; current is not null; current = current.Parent)
                {
                    switch (current)
                    {
                        case NamespaceDeclarationSyntax ns:
                            fullTypeName = ns.Name + "." + fullTypeName;
                            break;
                        case TypeDeclarationSyntax parentType:
                            fullTypeName = parentType.Identifier.ValueText + "." + fullTypeName;
                            break;
                    }
                }

                context.AddSource($"{fullTypeName}.IDeserialize.cs", Environment.NewLine + tree.ToFullString());
            }
        }

        private List<StatementSyntax>? GenerateDeserializeBody(ITypeSymbol typeSymbol)
        {
            var stmts = new List<StatementSyntax>();

            // Generate statements:
            // var visitor = new 'GeneratedVisitorName'();
            //
            // var fieldNames = new[] { 'field1', 'field2', 'field3' ... };
            // return deserializer.DeserializeType<'TypeName', 'GeneratedVisitorName'>('TypeName', fieldNames, visitor);

            // var visitor = new 'GeneratedVisitorName'();
            stmts.Add(LocalDeclarationStatement(
                VariableDeclaration(
                    IdentifierName("var"),
                    SeparatedList(new[] { VariableDeclarator(
                        Identifier("visitor"),
                        argumentList: null,
                        initializer: EqualsValueClause(ObjectCreationExpression(
                            IdentifierName(GeneratedVisitorName),
                            ArgumentList(),
                            initializer: null)))
                    })
                )));

            var serdeName = SerdeBuiltInName(typeSymbol.SpecialType);
            var typeSyntax = ParseTypeName(typeSymbol.ToString());
            if (serdeName is not null)
            {
                stmts.Add(ReturnStatement(InvocationExpression(
                    QualifiedName(
                        IdentifierName("deserializer"),
                        GenericName(
                            Identifier("Deserialize" + serdeName),
                            TypeArgumentList(SeparatedList(new TypeSyntax[] {
                                typeSyntax,
                                IdentifierName(GeneratedVisitorName)
                            }))
                        )
                    ),
                    ArgumentList(SeparatedList(new[] {
                        Argument(IdentifierName("visitor"))
                    }))
                )));
            }
            else
            {
                var members = SymbolUtilities.GetPublicDataMembers(typeSymbol);
                var namesArray = ImplicitArrayCreationExpression(InitializerExpression(
                    SyntaxKind.ArrayInitializerExpression,
                    SeparatedList(members.Select(d => (ExpressionSyntax)StringLiteral(d.Name)))));

                // var fieldNames = new[] { 'field1', 'field2', 'field3' ... };
                stmts.Add(LocalDeclarationStatement(
                    VariableDeclaration(
                        IdentifierName("var"),
                        SeparatedList(new[] { VariableDeclarator(
                        Identifier("fieldNames"),
                        argumentList: null,
                        initializer: EqualsValueClause(namesArray)) })
                    )
                ));

                // return deserializer.DeserializeType<'TypeName', 'GeneratedVisitorName'>('TypeName', fieldNames, visitor);
                stmts.Add(ReturnStatement(InvocationExpression(
                    QualifiedName(
                        IdentifierName("deserializer"),
                        GenericName(
                            Identifier("DeserializeType"),
                            TypeArgumentList(SeparatedList(new TypeSyntax[] {
                            IdentifierName(typeSymbol.Name),
                            IdentifierName(GeneratedVisitorName)
                            })))
                        ),
                    ArgumentList(SeparatedList(new[] {
                    Argument(StringLiteral(typeSymbol.Name)),
                    Argument(IdentifierName("fieldNames")),
                    Argument(IdentifierName("visitor"))
                    }))
                )));
            }

            return stmts;
        }

        private TypeDeclarationSyntax GenerateVisitor(ITypeSymbol type, TypeSyntax typeSyntax, GeneratorExecutionContext context)
        {
            // Serde.IDeserializeVisitor<'typeName'>
            var interfaceSyntax = QualifiedName(IdentifierName("Serde"), GenericName(
                Identifier("IDeserializeVisitor"),
                TypeArgumentList(SeparatedList(new[] { typeSyntax }))
            ));

            var typeName = typeSyntax.ToString();
            // Members:
            //     public string ExpectedTypeName => 'typeName';
            var property = PropertyDeclaration(
                attributeLists: default,
                modifiers: new SyntaxTokenList(new[] { Token(SyntaxKind.PublicKeyword) }),
                type: PredefinedType(Token(SyntaxKind.StringKeyword)),
                explicitInterfaceSpecifier: null,
                identifier: Identifier("ExpectedTypeName"),
                accessorList: null,
                expressionBody: ArrowExpressionClause(StringLiteral(typeName)),
                initializer: null,
                semicolonToken: Token(SyntaxKind.SemicolonToken));


            var serdeName = SerdeBuiltInName(type.SpecialType);
            string methodText;
            if (serdeName is not null)
            {
                methodText = $"{typeName} IDeserializeVisitor<{typeName}>.Visit{serdeName}({typeName} x) => x;";
            }
            else
            {
                var cases = new StringBuilder();
                var members = SymbolUtilities.GetPublicDataMembers(type);
                foreach (var m in members)
                {
                    string? wrapperName = null;
                    if (TryGetPrimitiveWrapper(m.Type) is { } primWrap)
                    {
                        wrapperName = primWrap;
                    }
                    else if (TryGetCompoundWrapper(m.Type, context, WrapUsage.Deserialize) is {} compound)
                    {
                        wrapperName = compound.ToString();
                    }
                    var memberType = m.Type.ToDisplayString();
                    cases.AppendLine(@$"
            case ""{m.Name}"":
                newType.{m.Name} = d.GetNextValue<{memberType}, {wrapperName}>();
                break;");
                }

                methodText = @$"
{typeName} Serde.IDeserializeVisitor<{typeName}>.VisitDictionary<D>(ref D d)
{{
    {typeName} newType = new {typeName}();
    while (d.TryGetNextKey<string, StringWrap>(out string? key))
    {{
        switch (key)
        {{
{cases.ToString()}
            default:
                throw new InvalidDeserializeValueException(""Unexpected field or property name in type {typeName}: '"" + key + ""'"");
        }}
    }}
    return newType;
}}";
            }

            var typeMembers = List(new MemberDeclarationSyntax[] {
                property,
                ParseMemberDeclaration(methodText)!
            });

            // private sealed class SerdeVisitor : IDeserializeVisitor<'typeName'>
            // {
            //     'Members'
            // }
            return ClassDeclaration(
                attributeLists: default,
                modifiers: new SyntaxTokenList(
                    Token(SyntaxKind.PrivateKeyword),
                    Token(SyntaxKind.SealedKeyword)),
                Identifier(GeneratedVisitorName),
                typeParameterList: null,
                baseList: BaseList(SeparatedList(new BaseTypeSyntax[] { SimpleBaseType(interfaceSyntax) })),
                constraintClauses: default,
                members: typeMembers);
        }
    }
}