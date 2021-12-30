
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

        private static (MemberDeclarationSyntax[], BaseListSyntax) GenerateDeserializeImpl(
            GeneratorExecutionContext context,
            ITypeSymbol receiverType,
            ExpressionSyntax receiverExpr)
        {
            TypeSyntax typeSyntax = ParseTypeName(receiverType.ToDisplayString());

            // `Serde.IDeserialize<'typeName'>
            var interfaceSyntax = QualifiedName(IdentifierName("Serde"), GenericName(
                Identifier("IDeserialize"),
                TypeArgumentList(SeparatedList(new[] { typeSyntax }))
            ));

            // Generate members for ISerialize.Deserialize implementation
            var method = GenerateDeserializeMethod(interfaceSyntax, receiverType);
            var visitorType = GenerateVisitor(receiverType, typeSyntax, context);
            var members = new MemberDeclarationSyntax[] { method, visitorType };
            var baseList = BaseList(SeparatedList(new BaseTypeSyntax[] { SimpleBaseType(interfaceSyntax) }));
            return (members, baseList);
        }

        // Generate method `void ISerialize.Deserialize(IDeserializer deserializer) { ... }`
        private static MethodDeclarationSyntax GenerateDeserializeMethod(
            QualifiedNameSyntax interfaceSyntax,
            ITypeSymbol typeSymbol)
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
                            typeSyntax,
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

            return MethodDeclaration(
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
                body: Block(stmts.ToArray()),
                expressionBody: null
                );
        }

        private static TypeDeclarationSyntax GenerateVisitor(ITypeSymbol type, TypeSyntax typeSyntax, GeneratorExecutionContext context)
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
                var locals = new StringBuilder();
                var assignments = new StringBuilder();
                var members = SymbolUtilities.GetPublicDataMembers(type);
                foreach (var m in members)
                {
                    string? wrapperName = null;
                    var memberType = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
                    if (ImplementsSerde(m.Type, context, SerdeUsage.Deserialize))
                    {
                        wrapperName = memberType;
                    }
                    else if (TryGetPrimitiveWrapper(m.Type) is {} primWrap)
                    {
                        wrapperName = primWrap;
                    }
                    else if (TryGetCompoundWrapper(m.Type, context, SerdeUsage.Deserialize) is {} compound)
                    {
                        wrapperName = compound.ToString();
                    }
                    else if (TryCreateWrapper(m.Type, m, context, SerdeUsage.Deserialize) is {} wrap)
                    {
                        wrapperName = wrap.ToString();
                    }
                    var lowerName = m.Name.ToLowerInvariant();
                    locals.AppendLine($"Serde.Option<{memberType}> {lowerName} = default;");
                    assignments.AppendLine($"{m.Name} = {lowerName}.Value,");
                    cases.AppendLine(@$"
            case ""{m.Name}"":
                {lowerName} = d.GetNextValue<{memberType}, {wrapperName}>();
                break;");
                }

                methodText = @$"
{typeName} Serde.IDeserializeVisitor<{typeName}>.VisitDictionary<D>(ref D d)
{{
    {locals}
    while (d.TryGetNextKey<string, StringWrap>(out string? key))
    {{
        switch (key)
        {{
{cases.ToString()}
            default:
                throw new InvalidDeserializeValueException(""Unexpected field or property name in type {typeName}: '"" + key + ""'"");
        }}
    }}
    {typeName} newType = new {typeName}() {{
        {assignments}
    }};
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