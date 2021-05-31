using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;

namespace Serde
{
    /// <summary>
    /// Recognizes the [GenerateSerde] attribute on a type to generate an implementation
    /// of Serde.ISerialize. The implementation generally looks like a call to SerializeType,
    /// then successive calls to SerializeField.
    /// </summary>
    /// <example>
    /// For a type like,
    ///
    /// <code>
    /// [GenerateSerde]
    /// partial struct Rgb { public byte Red; public byte Green; public byte Blue; }
    /// </code>
    ///
    /// The generated code would be,
    ///
    /// <code>
    /// using Serde;
    ///
    /// partial struct Rgb : Serde.ISerialize
    /// {
    ///     void Serde.ISerialize.Serialize&lt;TSerializer, TSerializeType&gt;TSerializer serializer)
    ///     {
    ///         var type = serializer.SerializeType("Rgb", 3);
    ///         type.SerializeField("Red", new ByteWrap(Red));
    ///         type.SerializeField("Green", new ByteWrap(Green));
    ///         type.SerializeField("Blue", new ByteWrap(Blue));
    ///         type.End();
    ///     }
    /// }
    /// </code>
    /// </example>
    [Generator]
    public class ISerializeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        internal static bool IsGenerateSerdeAnnotated(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.RecordDeclaration:
                    var typeDecl = (TypeDeclarationSyntax)node;
                    foreach (var attrLists in typeDecl.AttributeLists)
                    {
                        foreach (var attr in attrLists.Attributes)
                        {
                            var name = attr.Name;
                            while (name is QualifiedNameSyntax q)
                            {
                                name = q.Right;
                            }
                            if (name is IdentifierNameSyntax
                                {
                                    Identifier:
                                    {
                                        ValueText: "GenerateSerde" or "GenerateSerdeAttribute"
                                    }
                                })
                            {
                                return true;
                            }
                        }
                    }
                    break;
            }
            return false;
        }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                foreach (var node in tree.GetRoot().DescendantNodes())
                {
                    if (IsGenerateSerdeAnnotated(node))
                    {
                        GenerateImpl(new ProxyExecutionContext(context),
                            (TypeDeclarationSyntax)node,
                            context.Compilation.GetSemanticModel(tree));
                    }
                }
            }
        }

        internal readonly struct ProxyExecutionContext
        {
            private readonly Action<Diagnostic> _reportDiagnostic;
            private readonly Action<string, string> _addSource;
            public ProxyExecutionContext(GeneratorExecutionContext context)
            {
                _reportDiagnostic = context.ReportDiagnostic;
                _addSource = context.AddSource;
            }
            public ProxyExecutionContext(SourceProductionContext context)
            {
                _reportDiagnostic = context.ReportDiagnostic;
                _addSource = context.AddSource;
            }
            public void ReportDiagnostic(Diagnostic diagnostic) => _reportDiagnostic(diagnostic);
            public void AddSource(string hintName, string src) => _addSource(hintName, src);
        }

        internal static void GenerateImpl(
            ProxyExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel semanticModel)
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

            var typeName = typeDecl.Identifier.ValueText;

            var typeDef = semanticModel.GetDeclaredSymbol(typeDecl)!;
            var fieldsAndProps = typeDef.GetMembers().Where(m => m is {
                    DeclaredAccessibility: Accessibility.Public,
                    Kind: SymbolKind.Field or SymbolKind.Property,
                }).ToList();

            // Generate statements for ISerialize.Serialize implementation
            var statements = GenerateISerializeStatements(context, semanticModel.Compilation, typeName, fieldsAndProps);

            if (statements is not null)
            {
                // Generate method `void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer) { ... }`
                var newMethod = MethodDeclaration(
                    attributeLists: default,
                    modifiers: default,
                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                    explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(
                        QualifiedName(IdentifierName("Serde"), IdentifierName("ISerialize"))),
                    identifier: Identifier("Serialize"),
                    typeParameterList: TypeParameterList(SeparatedList(new[] {
                        "TSerializer", "TSerializeType", "TSerializeEnumerable"
                    }.Select(s => TypeParameter(s)))),
                    parameterList: ParameterList(SeparatedList(new[] { Parameter("TSerializer", "serializer", byRef: true) })),
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
        }

        private static List<StatementSyntax>? GenerateISerializeStatements(
            ProxyExecutionContext context,
            Compilation compilation,
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

                // Check if the type is a primitive
                if (TrySerializePrimitive(memberType, m))
                {
                    continue;
                }

                // Check if type implements Serde.ISerialize or has the GenerateSerde attribute
                var iserializeSymbol = compilation.GetTypeByMetadataName("Serde.ISerialize");
                if (HasGenerateSerdeAttribute(memberType.GetAttributes()) || memberType.Interfaces.Contains(iserializeSymbol, SymbolEqualityComparer.Default))
                {
                    AddSerializeField(memberType, IdentifierName(m.Name));
                    continue;
                }

                // No built-in handling and doesn't implement ISerialize, error
                context.ReportDiagnostic(CreateDiagnostic(DiagId.ERR_DoesntImplementISerialize, m.Locations[0], m, memberType));
                return null;

                bool HasGenerateSerdeAttribute(ImmutableArray<AttributeData> attributes)
                {
                    var attrType = compilation.GetTypeByMetadataName("Serde.GenerateSerdeAttribute");
                    if (attrType is null)
                    {
                        return false;
                    }
                    foreach (var attr in attributes)
                    {
                        if (attr.AttributeClass?.Equals(attrType, SymbolEqualityComparer.Default) == true)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            // `type.End();`
            statements.Add(ExpressionStatement(InvocationExpression(
                QualifiedName(IdentifierName("type"), IdentifierName("End")),
                ArgumentList()
            )));

            return statements;

            bool TrySerializePrimitive(ITypeSymbol type, ISymbol member)
            {
                // If the target is a core type, we need to wrap it. Otherwise, just pass it through
                string? wrapperName = type.SpecialType switch
                {
                    SpecialType.System_Boolean => "BoolWrap",
                    SpecialType.System_Char => "CharWrap",
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
                if (wrapperName is null)
                {
                    return false;
                }
                ExpressionSyntax fieldExpr = IdentifierName(member.Name);
                fieldExpr = ObjectCreationExpression(
                    IdentifierName(wrapperName),
                    ArgumentList(SeparatedList(new[] { Argument(fieldExpr) })),
                    initializer: null);

                AddSerializeField(member, fieldExpr);
                return true;
            }

            // Add a statement like `type.SerializeField("member.Name", value)`
            void AddSerializeField(ISymbol member, ExpressionSyntax value)
            {
                statements.Add(
                    ExpressionStatement(InvocationExpression(
                        // type.SerializeField
                        QualifiedName(IdentifierName("type"), IdentifierName("SerializeField")),
                        ArgumentList(SeparatedList(new ExpressionSyntax[] {
                            // "FieldName"
                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(member.Name)),
                            value
                        }.Select(Argument))))));
            }
        }

        private static ParameterSyntax Parameter(string typeName, string paramName, bool byRef = false) => SyntaxFactory.Parameter(
            attributeLists: default,
            modifiers: default,
            type: byRef ? SyntaxFactory.RefType(IdentifierName(typeName)) : IdentifierName(typeName),
            Identifier(paramName),
            default
        );

        private static LiteralExpressionSyntax NumericLiteral(int num)
            => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(num));

        private static LiteralExpressionSyntax StringLiteral(string text)
            => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(text));
    }
}