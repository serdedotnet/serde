
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.SerdeImplRoslynGenerator;

namespace Serde
{
    internal class DeserializeImplGenerator
    {
        private const string GeneratedVisitorName = "SerdeVisitor";

        internal static (MemberDeclarationSyntax[], BaseListSyntax) GenerateDeserializeImpl(
            GeneratorExecutionContext context,
            ITypeSymbol receiverType,
            ExpressionSyntax receiverExpr,
            ImmutableList<ITypeSymbol> inProgress)
        {
            TypeSyntax typeSyntax = ParseTypeName(receiverType.ToDisplayString());

            // `Serde.IDeserialize<'typeName'>
            var interfaceSyntax = QualifiedName(IdentifierName("Serde"), GenericName(
                Identifier("IDeserialize"),
                TypeArgumentList(SeparatedList(new[] { typeSyntax }))
            ));

            // Generate members for ISerialize.Deserialize implementation
            MemberDeclarationSyntax[] members;
            if (receiverType.TypeKind == TypeKind.Enum)
            {
                var method = GenerateOldDeserializeMethod(context, interfaceSyntax, receiverType);
                var visitorType = GenerateVisitor(receiverType, typeSyntax, context, inProgress);
                members = [ method, visitorType ];
            }
            else
            {
                var method = GenerateCustomDeserializeMethod(context, receiverType, typeSyntax, inProgress);
                members = [ method ];
            }
            var baseList = BaseList(SeparatedList(new BaseTypeSyntax[] { SimpleBaseType(interfaceSyntax) }));
            return (members, baseList);
        }

        /// <summary>
        /// Generates
        /// <code>
        /// T IDeserialize&lt;T&gt;.Deserialize(IDeserializer deserializer)
        /// {
        ///     var _local1 = default!;
        ///     ...
        ///     var _localN = default!;
        ///
        ///     var typeInfo = {typeName}SerdeTypeInfo.TypeInfo;
        ///     var typDeserializer = deserializer.DeserializeType(typeInfo);
        ///     int index;
        ///     while ((index = typeDeserialize.TryReadIndex(typeInfo)) != IDeserializeType.EndOfType)
        ///     {
        ///         switch (index)
        ///         {
        ///         }
        ///     }
        /// }
        /// </code>
        /// </summary>
        private static MethodDeclarationSyntax GenerateCustomDeserializeMethod(
            GeneratorExecutionContext context,
            ITypeSymbol type,
            TypeSyntax typeSyntax,
            ImmutableList<ITypeSymbol> inProgress)
        {
            Debug.Assert(type.TypeKind != TypeKind.Enum);

            var members = SymbolUtilities.GetDataMembers(type, SerdeUsage.Both);
            var typeFqn = typeSyntax.ToString();
            var assignedVarType = members.Count switch {
                <= 8 => "byte",
                <= 16 => "ushort",
                <= 32 => "uint",
                <= 64 => "ulong",
                _ => throw new InvalidOperationException("Too many members in type")
            };
            var (cases, locals, requiredMask) = InitCasesAndLocals();
            string typeCreationExpr = GenerateTypeCreation(context, typeFqn, type, members, requiredMask);

            const string typeInfoLocalName = "_l_typeInfo";
            const string indexLocalName = "_l_index_";

            var methodText = $$"""
static {{typeFqn}} Serde.IDeserialize<{{typeFqn}}>.Deserialize(IDeserializer deserializer)
{
    {{locals}}
    {{assignedVarType}} {{AssignedVarName}} = 0;

    var {{typeInfoLocalName}} = {{type.Name}}SerdeTypeInfo.TypeInfo;
    var typeDeserialize = deserializer.DeserializeType({{typeInfoLocalName}});
    int {{indexLocalName}};
    while (({{indexLocalName}} = typeDeserialize.TryReadIndex({{typeInfoLocalName}}, out var _l_errorName)) != IDeserializeType.EndOfType)
    {
        switch ({{indexLocalName}})
        {
{{cases}}
        }
    }
    {{typeCreationExpr}}
    return newType;
}
""";
            return (MethodDeclarationSyntax)ParseMemberDeclaration(methodText)!;

            (string Cases, string Locals, string AssignedMask) InitCasesAndLocals()
            {
                var casesBuilder = new StringBuilder();
                var localsBuilder = new StringBuilder();
                long assignedMaskValue = 0;
                var skippedIndices = new List<int>();
                for (int i = 0; i < members.Count; i++)
                {
                    if (members[i].SkipDeserialize)
                    {
                        skippedIndices.Add(i);
                        continue;
                    }

                    var m = members[i];
                    string wrapperName;
                    var memberType = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
                    if (TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize, inProgress) is { } explicitWrap)
                    {
                        wrapperName = explicitWrap.ToString();
                    }
                    else if (ImplementsSerde(m.Type, context, SerdeUsage.Deserialize))
                    {
                        wrapperName = memberType;
                    }
                    else if (TryGetAnyWrapper(m.Type, context, SerdeUsage.Deserialize, inProgress) is { } wrap)
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
                    case {i}:
                        {localName} = typeDeserialize.ReadValue<{memberType}, {wrapperName}>({indexLocalName});
                        {AssignedVarName} |= (({assignedVarType})1) << {i};
                        break;
                    """);
                    if (!m.IsNullable || m.ThrowIfMissing)
                    {
                        assignedMaskValue |= 1L << i;
                    }
                }
                var unknownMemberBehavior = SymbolUtilities.GetTypeOptions(type).DenyUnknownMembers
                    ? $"""
                    throw new InvalidDeserializeValueException("Unexpected field or property name in type {type.Name}: '" + _l_errorName + "'");
                    """
                    : "break;";
                foreach (var i in skippedIndices)
                {
                    casesBuilder.AppendLine($"""
                    case {i}:
                    """);
                }
                casesBuilder.AppendLine($"""
                    case Serde.IDeserializeType.IndexNotFound:
                        {unknownMemberBehavior}
                """);
                casesBuilder.AppendLine($"""
                    default:
                        throw new InvalidOperationException("Unexpected index: " + {indexLocalName});
                    """);
                return (casesBuilder.ToString(),
                        localsBuilder.ToString(),
                        "0b" + Convert.ToString(assignedMaskValue, 2));
            }

        }

        // This is the old visitor-driven deserialization method. It is being replaced by the new
        // TypeInfo-driven deserialization.
        // Generate method `void ISerialize.Deserialize(IDeserializer deserializer) { ... }`
        private static MethodDeclarationSyntax GenerateOldDeserializeMethod(
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
            // return deserializer.DeserializeType('TypeName', fieldNames, visitor);

            var serdeName = SerdeImplRoslynGenerator.SerdeBuiltInName(typeSymbol.SpecialType);
            var typeSyntax = ParseTypeName(typeSymbol.ToString());
            if (serdeName is not null)
            {
                // Built-in type
                stmts.Add(ReturnStatement(InvocationExpression(
                    QualifiedName(
                        IdentifierName("deserializer"),
                        IdentifierName("Deserialize" + serdeName)
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
                        IdentifierName("DeserializeString")
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

                // return deserializer.DeserializeType('TypeName', fieldNames, visitor);
                stmts.Add(ReturnStatement(InvocationExpression(
                    QualifiedName(
                        IdentifierName("deserializer"),
                        IdentifierName("DeserializeType")),
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
                typeParameterList: null,
                parameterList: ParameterList(SeparatedList(new[] { Parameter("IDeserializer", "deserializer", byRef: false) })),
                constraintClauses: default,
                body: Block(stmts.ToArray()),
                expressionBody: null
                );
        }

        private static TypeDeclarationSyntax GenerateVisitor(
            ITypeSymbol type,
            TypeSyntax typeSyntax,
            GeneratorExecutionContext context,
            ImmutableList<ITypeSymbol> inProgress)
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
                typeMembers.Add(GenerateCustomTypeVisitor(type, typeName, context, members, inProgress));
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
private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
{
    public static readonly FieldNameVisitor Instance = new FieldNameVisitor();
    public static byte Deserialize(IDeserializer deserializer)
        => deserializer.DeserializeString(Instance);

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

        private static MemberDeclarationSyntax GenerateCustomTypeVisitor(
            ITypeSymbol type,
            string typeName,
            GeneratorExecutionContext context,
            List<DataMemberSymbol> members,
            ImmutableList<ITypeSymbol> inProgress)
        {
            var assignedVarType = members.Count switch {
                <= 8 => "byte",
                <= 16 => "ushort",
                <= 32 => "uint",
                <= 64 => "ulong",
                _ => throw new InvalidOperationException("Too many members in type")
            };
            var (cases, locals, assignedMask) = InitCasesAndLocals();
            string typeCreationExpr = GenerateTypeCreation(context, typeName, type, members, assignedMask);
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

            (string Cases, string Locals, string AssignedMask) InitCasesAndLocals()
            {
                var casesBuilder = new StringBuilder();
                var localsBuilder = new StringBuilder();
                long assignedMaskValue = 0;
                for (int i = 0; i < members.Count; i++)
                {
                    var m = members[i];
                    string wrapperName;
                    var memberType = m.Type.WithNullableAnnotation(m.NullableAnnotation).ToDisplayString();
                    if (TryGetExplicitWrapper(m, context, SerdeUsage.Deserialize, inProgress) is { } explicitWrap)
                    {
                        wrapperName = explicitWrap.ToString();
                    }
                    else if (ImplementsSerde(m.Type, context, SerdeUsage.Deserialize))
                    {
                        wrapperName = memberType;
                    }
                    else if (TryGetAnyWrapper(m.Type, context, SerdeUsage.Deserialize, inProgress) is { } wrap)
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
                return (casesBuilder.ToString(),
                        localsBuilder.ToString(),
                        "0b" + Convert.ToString(assignedMaskValue, 2));
            }
        }

        /// <summary>
        /// If the type has a parameterless constructor then we will use that and just set
        /// each member in the initializer. If there is no parameterlss constructor, there
        /// must be a constructor signature as specified by the ConstructorSignature property
        /// in the SerdeTypeOptions.
        /// </summary>
        private static string GenerateTypeCreation(
            GeneratorExecutionContext context,
            string typeName,
            ITypeSymbol type,
            List<DataMemberSymbol> members,
            string assignedMask)
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
                if (m.SkipDeserialize)
                {
                    continue;
                }
                assignments.AppendLine($"{m.Name} = {GetLocalName(m)},");
            }
            var mask = new string('1', members.Count);
            return $$"""
    if (({{AssignedVarName}} & {{assignedMask}}) != {{assignedMask}})
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