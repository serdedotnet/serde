using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.WellKnownTypes;

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
    ///     void Serde.ISerialize.Serialize&lt;TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary&gt;TSerializer serializer)
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
    public class SerializeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                foreach (var typeDecl in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    if (typeDecl.IsKind(SyntaxKind.ClassDeclaration)
                         || typeDecl.IsKind(SyntaxKind.StructDeclaration)
                         || typeDecl.IsKind(SyntaxKind.RecordDeclaration))
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
                                if (name is IdentifierNameSyntax
                                    {
                                        Identifier:
                                        {
                                            ValueText: "GenerateSerde" or "GenerateSerdeAttribute"
                                        }
                                    })
                                {
                                    VisitType(context, typeDecl, context.Compilation.GetSemanticModel(tree));
                                }
                            }
                        }
                    }

                }
            }
        }

        private void VisitType(
            GeneratorExecutionContext context,
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
            var statements = GenerateSerializeBody(context, typeName, fieldsAndProps);

            if (statements is not null)
            {
                // Generate method `void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer) { ... }`
                var newMethod = MethodDeclaration(
                    attributeLists: default,
                    modifiers: default,
                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                    explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(
                        QualifiedName(IdentifierName("Serde"), IdentifierName("ISerialize"))),
                    identifier: Identifier("Serialize"),
                    typeParameterList: TypeParameterList(SeparatedList(new[] {
                        "TSerializer", "TSerializeType", "TSerializeEnumerable", "TSerializeDictionary"
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
                    .WithSemicolonToken(default)
                    .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                    .WithMembers(List(new MemberDeclarationSyntax[] { newMethod }))
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

                string fullTypeName = typeName;
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

                context.AddSource($"{fullTypeName}.ISerialize.cs", Environment.NewLine + tree.ToFullString());
            }
        }

        private List<StatementSyntax>? GenerateSerializeBody(
            GeneratorExecutionContext context,
            string typeName,
            List<ISymbol> fieldsAndProps)
        {
            // The generated body of ISerialize is
            // `var type = serializer.SerializeType("TypeName", numFields)`
            // type.SerializeField("FieldName", FieldValue);
            // type.End();

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

                ExpressionSyntax? expr;
                // Always prefer a direct implementation of ISerialize
                if (ImplementsISerialize(memberType, context))
                {
                    expr = IdentifierName(m.Name);
                    statements.Add(MakeSerializeField(m, expr));
                    continue;
                }

                // Check if the type is a primitive
                if (TrySerializeBuiltIn(memberType, m, context, out expr))
                {
                    statements.Add(MakeSerializeField(m, expr));
                    continue;
                }

                // No built-in handling and doesn't implement ISerializable, error
                context.ReportDiagnostic(CreateDiagnostic(DiagId.ERR_DoesntImplementISerializable, m.Locations[0], m, memberType));
                return null;


                // Add a statement like `type.SerializeField("member.Name", value)`
                static ExpressionStatementSyntax MakeSerializeField(ISymbol member, ExpressionSyntax value)
                    => ExpressionStatement(InvocationExpression(
                        // type.SerializeField
                        QualifiedName(IdentifierName("type"), IdentifierName("SerializeField")),
                        ArgumentList(SeparatedList(new ExpressionSyntax[] {
                            // "FieldName"
                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(member.Name)),
                            value
                        }.Select(Argument)))));
            }

            // `type.End();`
            statements.Add(ExpressionStatement(InvocationExpression(
                QualifiedName(IdentifierName("type"), IdentifierName("End")),
                ArgumentList()
            )));

            return statements;
        }

        /// <summary>
        /// SerdeDn provides wrappers for primitives and common types in the framework. If found,
        /// we generate and initialize the wrapper.
        /// </summary>
        private static bool TrySerializeBuiltIn(
            ITypeSymbol type,
            ISymbol member,
            GeneratorExecutionContext context,
            [NotNullWhen(true)] out ExpressionSyntax? fieldExpr)
        {
            var wrapperName = TryGetPrimitiveWrapper(type);
            if (wrapperName is not null)
            {
                fieldExpr = ObjectCreationExpression(
                    IdentifierName(wrapperName),
                    ArgumentList(SeparatedList(new[] { Argument(IdentifierName(member.Name)) })),
                    initializer: null);
                return true;
            }

            var wrapperType = TryGetCompoundWrapper(type, context);
            if (wrapperType is not null)
            {
                fieldExpr = ObjectCreationExpression(
                    wrapperType,
                    ArgumentList(SeparatedList(new[] { Argument(IdentifierName(member.Name)) })),
                    initializer: null);
                return true;
            }
            fieldExpr = null;
            return false;
        }

        // If the target is a core type, we can wrap it
        private static string? TryGetPrimitiveWrapper(ITypeSymbol type) => type.SpecialType switch
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

        private static TypeSyntax? TryGetCompoundWrapper(ITypeSymbol type, GeneratorExecutionContext context)
        {
            switch (type)
            {
                case IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType }:
                    return MakeWrappedExpression("ArrayWrap", ImmutableArray.Create(elemType), context);

                case INamedTypeSymbol t:
                    if (TryGetWrapperName(t, context) is {} tuple)
                    {
                        return MakeWrappedExpression(tuple.WrapperName, tuple.Args, context);
                    }
                    break;
            }
            return null;
        }

        private static TypeSyntax? MakeWrappedExpression(
            string baseWrapperName,
            ImmutableArray<ITypeSymbol> elemTypes,
            GeneratorExecutionContext context)
        {
            var wrapperTypes = new List<TypeSyntax>();
            foreach (var elemType in elemTypes)
            {
                var elemTypeSyntax = ParseTypeName(elemType.ToDisplayString());

                if (ImplementsISerialize(elemType, context))
                {
                    // Special case for List-like types: 
                    // If the element type directly implements ISerialize, we can
                    // use a single-arity version of the wrapper
                    //      ArrayWrap<`elemType`>
                    wrapperTypes.Add(elemTypeSyntax);

                    // Otherwise we need an `IdWrap` which just delegates to the inner
                    // type.
                    if (elemTypes.Length > 1)
                    {
                        wrapperTypes.Add(GenericName(
                            Identifier("IdWrap"), TypeArgumentList(SeparatedList(new TypeSyntax[] {
                                elemTypeSyntax
                            }))
                        ));
                    }
                    continue;
                }

                // Otherwise we'll need to wrap the element type as well e.g.,
                //      ArrayWrap<`elemType`, `elemTypeWrapper`>

                var primWrapper = TryGetPrimitiveWrapper(elemType);
                TypeSyntax? wrapperName = primWrapper is not null
                    ? IdentifierName(primWrapper)
                    : TryGetCompoundWrapper(elemType, context);

                if (wrapperName is null)
                {
                    // Could not find a wrapper
                    return null;
                }
                else
                {
                    wrapperTypes.Add(elemTypeSyntax);
                    wrapperTypes.Add(wrapperName);
                }
            }

            return GenericName(
                Identifier(baseWrapperName), TypeArgumentList(SeparatedList(wrapperTypes)));
        }

        private static (string WrapperName, ImmutableArray<ITypeSymbol> Args)? TryGetWrapperName(INamedTypeSymbol typeSymbol, GeneratorExecutionContext context)
        {
            if (TryGetWellKnownType(typeSymbol, context) is {} wk)
            {
                return (wk.ToWrapper(), typeSymbol.TypeArguments);
            }
            // Check if it implements well-known interfaces
            foreach (var iface in WellKnownTypes.GetAvailableInterfacesInOrder(context))
            {
                Debug.Assert(iface.TypeKind == TypeKind.Interface);
                foreach (var impl in typeSymbol.Interfaces)
                {
                    if (impl.OriginalDefinition.Equals(iface, SymbolEqualityComparer.Default) &&
                        WellKnownTypes.TryGetWellKnownType(iface, context)?.ToWrapper() is { } wrap)
                    {
                        return (wrap, impl.TypeArguments);
                    }
                }
            }
            return null;
        }

        static bool ImplementsISerialize(ITypeSymbol memberType, GeneratorExecutionContext context)
        {
            // Check if the type either has the GenerateSerde attribute, or directly implements ISerialize
            // (If the type has the GenerateSerde attribute then the generator will implement the interface)
            var attributes = memberType.GetAttributes();
            foreach (var attr in attributes)
            {
                if (attr.AttributeClass?.Name is "GenerateSerde" or "GenerateSerdeAttribute")
                {
                    return true;
                }
            }

            var iserializeSymbol = context.Compilation.GetTypeByMetadataName("Serde.ISerialize");
            if (iserializeSymbol is not null && memberType.Interfaces.Contains(iserializeSymbol, SymbolEqualityComparer.Default))
            {
                return true;
            }
            return false;
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