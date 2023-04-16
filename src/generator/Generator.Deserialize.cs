
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
    partial class SerdeImplRoslynGenerator
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
            var method = GenerateDeserializeMethod(context, interfaceSyntax, receiverType);
            var visitorType = GenerateVisitor(receiverType, typeSyntax, context);
            var members = new MemberDeclarationSyntax[] { method, visitorType };
            var baseList = BaseList(SeparatedList(new BaseTypeSyntax[] { SimpleBaseType(interfaceSyntax) }));
            return (members, baseList);
        }

        // Generate method `void ISerialize.Deserialize(IDeserializer deserializer) { ... }`
        private static MethodDeclarationSyntax GenerateDeserializeMethod(
            GeneratorExecutionContext context,
            QualifiedNameSyntax interfaceSyntax,
            ITypeSymbol typeSymbol)
        {
            var stmts = new List<StatementSyntax>();

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


            // Generate statements:
            // var visitor = new 'GeneratedVisitorName'();

            // Three options:
            // 1. Built-in type
            // 2. Enum type
            // 3. Custom type
            //
            // var fieldNames = new[] { 'field1', 'field2', 'field3' ... };
            // return deserializer.DeserializeType<'TypeName', 'GeneratedVisitorName'>('TypeName', fieldNames, visitor);

            var serdeName = SerdeBuiltInName(typeSymbol.SpecialType);
            var typeSyntax = ParseTypeName(typeSymbol.ToString());
            if (serdeName is not null)
            {
                // Built-in type
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
            else if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                // 2. Enum type
                stmts.Add(ReturnStatement(InvocationExpression(
                    QualifiedName(
                        IdentifierName("deserializer"),
                        GenericName(
                            Identifier("DeserializeString"),
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
                // 3. Custom type
                var members = SymbolUtilities.GetDataMembers(typeSymbol, SerdeUsage.Deserialize);
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
            var typeMembers = new List<MemberDeclarationSyntax>();
            // Members:
            //     public string ExpectedTypeName => 'typeName';
            typeMembers.Add(PropertyDeclaration(
                attributeLists: default,
                modifiers: new SyntaxTokenList(new[] { Token(SyntaxKind.PublicKeyword) }),
                type: PredefinedType(Token(SyntaxKind.StringKeyword)),
                explicitInterfaceSpecifier: null,
                identifier: Identifier("ExpectedTypeName"),
                accessorList: null,
                expressionBody: ArrowExpressionClause(StringLiteral(typeName)),
                initializer: null,
                semicolonToken: Token(SyntaxKind.SemicolonToken)));

            // Three cases
            // 1. Built-in type
            // 2. Enum type
            // 3. Custom type

            var serdeName = SerdeBuiltInName(type.SpecialType);
            if (serdeName is not null)
            {
                var methodText = $"{typeName} IDeserializeVisitor<{typeName}>.Visit{serdeName}({typeName} x) => x;";
                typeMembers.Add(ParseMemberDeclaration(methodText)!);
            }
            else if (type.TypeKind == TypeKind.Enum)
            {
                var stringCases = new StringBuilder();
                var spanCases = new StringBuilder();
                foreach (var m in SymbolUtilities.GetDataMembers(type, SerdeUsage.Deserialize))
                {
                    var formatted = m.GetFormattedName();
                    stringCases.Append($$"""
        "{{formatted}}" => {{typeName}}.{{m.Name}},
""");
                    spanCases.Append($$"""
        _ when System.MemoryExtensions.SequenceEqual(s, "{{formatted}}"u8) => {{typeName}}.{{m.Name}},
""");
                }
                var methodText = $$"""
{{typeName}} Serde.IDeserializeVisitor<{{typeName}}>.VisitString(string s) => s switch
    {
{{stringCases}}
        _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)
    };
""";

                var spanMethodText = $$"""
{{typeName}} Serde.IDeserializeVisitor<{{typeName}}>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
    {
{{spanCases}}
        _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))
    };
""";
                typeMembers.Add(ParseMemberDeclaration(methodText)!);
                typeMembers.Add(ParseMemberDeclaration(spanMethodText)!);
            }
            else
            {
                var members = SymbolUtilities.GetDataMembers(type, SerdeUsage.Deserialize);
                typeMembers.Add(GenerateFieldNameVisitor(type, typeName, members));
                typeMembers.Add(GenerateCustomTypeVisitor(type, typeName, context, members));
            }

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
                members: List(typeMembers));
        }

        private static MemberDeclarationSyntax GenerateFieldNameVisitor(ITypeSymbol type, string typeName, List<DataMemberSymbol> members)
        {
            var text = $$"""
private struct FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
{
    public static byte Deserialize<D>(ref D deserializer) where D : IDeserializer
        => deserializer.DeserializeString<byte, FieldNameVisitor>(new FieldNameVisitor());

    public string ExpectedTypeName => "string";

    byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
    public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
    {
        switch (s[0])
        {
            {{GetSwitchCases()}}
        }
    }
}
""";
            string GetSwitchCases()
            {
                var cases = new StringBuilder();
                for (int i = 0; i < members.Count; i++)
                {
                    var m = members[i];
                    var formatted = m.GetFormattedName();
                    cases.AppendLine($"""
                        case (byte)'{formatted[0]}' when s.SequenceEqual("{formatted}"u8):
                            return {i+1};

                        """);
                }
                string unknownMemberBehavior = SymbolUtilities.GetTypeOptions(type).DenyUnknownMembers
                    ? $"throw new InvalidDeserializeValueException(\"Unexpected field or property name in type {typeName}: '\" + System.Text.Encoding.UTF8.GetString(s) + \"'\");"
                    : "return 0;";
                cases.AppendLine($"""
                    default:
                        {unknownMemberBehavior}
                    """);
                return cases.ToString();
            }

            return ParseMemberDeclaration(text)!;
        }

        private const string AssignedVarName = "_r_assignedValid";

        private static MemberDeclarationSyntax GenerateCustomTypeVisitor(ITypeSymbol type, string typeName, GeneratorExecutionContext context, List<DataMemberSymbol> members)
        {
            var assignedVarType = members.Count switch {
                <= 8 => "byte",
                <= 16 => "ushort",
                <= 32 => "uint",
                <= 64 => "ulong",
                _ => throw new InvalidOperationException("Too many members in type")
            };
            string cases;
            string locals;
            string assignedMask;
            InitCasesAndLocals();
            string typeCreationExpr = GenerateTypeCreation(context, typeName, type, members);
            var methodText = $$"""
{{typeName}} Serde.IDeserializeVisitor<{{typeName}}>.VisitDictionary<D>(ref D d)
{
    {{locals}}
    {{assignedVarType}} {{AssignedVarName}} = {{assignedMask}};
    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
    {
        switch (key)
        {
{{cases}}
        }
    }
    {{typeCreationExpr}}
    return newType;
}
""";
            return ParseMemberDeclaration(methodText)!;

            void InitCasesAndLocals()
            {
                var casesBuilder = new StringBuilder();
                var localsBuilder = new StringBuilder();
                long assignedMaskValue = 0;
                for (int i = 0; i < members.Count; i++)
                {
                    var m = members[i];
                    string wrapperName;
                    var memberType = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
                    if (TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize) is { } explicitWrap)
                    {
                        wrapperName = explicitWrap.ToString();
                    }
                    else if (ImplementsSerde(m.Type, context, SerdeUsage.Deserialize))
                    {
                        wrapperName = memberType;
                    }
                    else if (TryGetAnyWrapper(m.Type, context, SerdeUsage.Deserialize) is { } wrap)
                    {
                        wrapperName = wrap.ToString();
                    }
                    else
                    {
                        // No built-in handling and doesn't implement IDeserialize, error
                        context.ReportDiagnostic(CreateDiagnostic(
                            DiagId.ERR_DoesntImplementInterface,
                            m.Locations[0],
                            m.Symbol,
                            memberType,
                            "Serde.IDeserialize"));
                        wrapperName = memberType;
                    }
                    var localName = GetLocalName(m);
                    localsBuilder.AppendLine($"{memberType} {localName} = default!;");
                    casesBuilder.AppendLine($"""
                    case {i + 1}:
                        {localName} = d.GetNextValue<{memberType}, {wrapperName}>();
                        {AssignedVarName} |= (({assignedVarType})1) << {i};
                        break;
                    """);
                    if (m.IsNullable && !m.ThrowIfMissing)
                    {
                        assignedMaskValue |= 1L << i;
                    }
                }
                cases = casesBuilder.ToString();
                locals = localsBuilder.ToString();
                assignedMask = "0b" + Convert.ToString(assignedMaskValue, 2);
            }
        }

        /// <summary>
        /// If the type has a parameterless constructor then we will use that and just set
        /// each member in the initializer. If there is no parameterlss constructor, there
        /// must be a constructor signature as specified by the ConstructorSignature property
        /// in the SerdeTypeOptions.
        /// </summary>
        private static string GenerateTypeCreation(GeneratorExecutionContext context, string typeName, ITypeSymbol type, List<DataMemberSymbol> members)
        {
            var targetSignature = SymbolUtilities.GetTypeOptions(type).ConstructorSignature;
            var targetTuple = targetSignature as INamedTypeSymbol;
            var ctors = type.GetMembers(".ctor");
            IMethodSymbol? targetCtor = null;
            IMethodSymbol? parameterLessCtor = null;
            foreach (var ctorSymbol in ctors)
            {
                if (ctorSymbol is IMethodSymbol ctorMethod)
                {
                    if (targetTuple is not null && ctorMethod.Parameters.Length == targetTuple.TupleElements.Length)
                    {
                        bool mismatch = false;
                        for(int i = 0; i < targetTuple.TupleElements.Length; i++)
                        {
                            var elem = targetTuple.TupleElements[i];
                            var param = ctorMethod.Parameters[i];
                            if (!elem.Type.Equals(param.Type, SymbolEqualityComparer.Default))
                            {
                                mismatch = true;
                                break;
                            }
                        }
                        if (!mismatch)
                        {
                            targetCtor = ctorMethod;
                            break;
                        }
                    }
                    if (ctorMethod is { Parameters.Length: 0 })
                    {
                        parameterLessCtor = ctorMethod;
                        if (targetSignature is null)
                        {
                            break;
                        }
                    }
                }
            }
            if (targetSignature is not null && targetCtor is null)
            {
                context.ReportDiagnostic(CreateDiagnostic(DiagId.ERR_CantFindConstructorSignature, type.Locations[0]));
                return "";
            }

            var assignmentMembers = new List<DataMemberSymbol>(members);
            var assignments = new StringBuilder();
            var parameters = new StringBuilder();
            if (targetCtor is not null)
            {
                foreach (var p in targetCtor.Parameters)
                {
                    var index = assignmentMembers.FindIndex(m => m.Name == p.Name);
                    if (parameters.Length != 0)
                    {
                        parameters.Append(", ");
                    }
                    parameters.Append(GetLocalName(assignmentMembers[index]));
                    assignmentMembers.RemoveAt(index);
                }
            }

            foreach (var m in assignmentMembers)
            {
                assignments.AppendLine($"{m.Name} = {GetLocalName(m)},");
            }
            var mask = new string('1', members.Count);
            return $$"""
    if ({{AssignedVarName}} != 0b{{mask}})
    {
        throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
    }
    var newType = new {{typeName}}({{parameters}}) {
        {{assignments}}
    };
""";
        }

        private static string GetLocalName(DataMemberSymbol m) => "_l_" + m.Name.ToLower();
    }
}