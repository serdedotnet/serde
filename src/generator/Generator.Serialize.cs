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
    public partial class Generator : ISourceGenerator
    {
        private void GenerateSerialize(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel semanticModel)
        {
            GenerateSerialize(context, typeDecl, semanticModel, receiverName: null);
        }

        private void GenerateSerialize(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel semanticModel,
            string? receiverName)
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

            // Generate statements for ISerialize.Serialize implementation
            var statements = GenerateSerializeBody(context, typeDecl, semanticModel, receiverName);

            if (statements is not null)
            {
                // Generate method `void ISerialize.Serialize(ISerializer serializer) { ... }`
                var newMethod = MethodDeclaration(
                    attributeLists: default,
                    modifiers: default,
                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                    explicitInterfaceSpecifier: ExplicitInterfaceSpecifier(
                        QualifiedName(IdentifierName("Serde"), IdentifierName("ISerialize"))),
                    identifier: Identifier("Serialize"),
                    typeParameterList: TypeParameterList(SeparatedList(new[] {
                        TypeParameter("TSerializer"),
                        TypeParameter("TSerializeType"),
                        TypeParameter("TSerializeEnumerable"),
                        TypeParameter("TSerializeDictionary")
                        })),
                    parameterList: ParameterList(SeparatedList(new[] { Parameter("TSerializer", "serializer", byRef: true) })),
                    constraintClauses: default,
                    body: Block(statements.ToArray()),
                    expressionBody: null
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

                context.AddSource($"{fullTypeName}.cs", Environment.NewLine + tree.ToFullString());
            }
        }

        private List<StatementSyntax>? GenerateSerializeBody(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel model,
            string? receiverName)
        {
            ExpressionSyntax receiverExpr;
            ITypeSymbol receiverType;
            if (receiverName is null)
            {
                receiverType = model.GetDeclaredSymbol(typeDecl)!;
                receiverExpr = ThisExpression();
            }
            else
            {
                var members = model.LookupSymbols(typeDecl.SpanStart, name: receiverName);
                if (members.Length != 1)
                {
                    return null;
                }
                receiverType = SymbolUtilities.GetSymbolType(members[0]);
                receiverExpr = IdentifierName(receiverName);
            }

            var statements = new List<StatementSyntax>();

            // If this is one of the serde-dn types, dispatch directly
            if (receiverType.SpecialType == SpecialType.System_String)
            {
                // `serializer.SerializeString(receiver)`
                statements.Add(ExpressionStatement(InvocationExpression(
                    QualifiedName(IdentifierName("serializer"), IdentifierName("SerializeString")),
                    ArgumentList(SeparatedList(new[] { Argument(receiverExpr) }))
                )));
                return statements;
            }

            var fieldsAndProps = receiverType.GetMembers()
                .Where(m => m is {
                    DeclaredAccessibility: Accessibility.Public,
                    Kind: SymbolKind.Field or SymbolKind.Property,
                })
                .Select(m => new DataMemberSymbol(m)).ToList();

            // The generated body of ISerialize is
            // `var type = serializer.SerializeType("TypeName", numFields)`
            // type.SerializeField("FieldName", receiver.FieldValue);
            // type.End();

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
                                Argument(StringLiteral(receiverType.Name)), Argument(NumericLiteral(fieldsAndProps.Count))
                            }))
                        ))
                    )
                })
            )));

            foreach (var m in fieldsAndProps)
            {
                var memberType = m.Type;
                // Generate statements of the form `type.SerializeField("FieldName", FieldValue)`
                var expr = TryMakeSerializeFieldExpr(m, memberType, context, receiverExpr);
                if (expr is null)
                {
                    // No built-in handling and doesn't implement ISerializable, error
                    context.ReportDiagnostic(CreateDiagnostic(DiagId.ERR_DoesntImplementISerializable, m.Locations[0], m, memberType));
                }
                else
                {
                    statements.Add(MakeSerializeFieldStmt(m, expr));
                }
            }

            // `type.End();`
            statements.Add(ExpressionStatement(InvocationExpression(
                QualifiedName(IdentifierName("type"), IdentifierName("End")),
                ArgumentList()
            )));

            return statements;

            static ExpressionSyntax? TryMakeSerializeFieldExpr(
                DataMemberSymbol m,
                ITypeSymbol memberType,
                GeneratorExecutionContext context,
                ExpressionSyntax receiverExpr)
            {
                // Always prefer a direct implementation of ISerialize
                if (ImplementsISerialize(memberType, context))
                {
                    return MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        receiverExpr,
                        IdentifierName(m.Name));
                }

                // Try using a built-in wrapper
                return TrySerializeBuiltIn(memberType, m, context);
            }

            // Make a statement like `type.SerializeField("member.Name", value)`
            static ExpressionStatementSyntax MakeSerializeFieldStmt(DataMemberSymbol member, ExpressionSyntax value)
                => ExpressionStatement(InvocationExpression(
                    // type.SerializeField
                    QualifiedName(IdentifierName("type"), IdentifierName("SerializeField")),
                    ArgumentList(SeparatedList(new ExpressionSyntax[] {
                        // "FieldName"
                        LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(member.Name)),
                        value
                    }.Select(Argument)))));
        }

        /// <summary>
        /// SerdeDn provides wrappers for primitives and common types in the framework. If found,
        /// we generate and initialize the wrapper.
        /// </summary>
        private static ExpressionSyntax? TrySerializeBuiltIn(
            ITypeSymbol type,
            DataMemberSymbol member,
            GeneratorExecutionContext context)
        {
            var argListFromMemberName = ArgumentList(SeparatedList(new[] { Argument(IdentifierName(member.Name)) }));
            // Priority:

            // 1. an explicit wrap

            var attr = TryGetExplicitWrapper(member);
            if (attr is { ConstructorArguments: { Length: 1 } attrArgs } &&
                attrArgs[0] is { Value: INamedTypeSymbol wrapperType })
            {
                var memberType = member.Type;
                var typeArgs = memberType switch
                {
                    INamedTypeSymbol n => n.TypeArguments,
                    _ => ImmutableArray<ITypeSymbol>.Empty
                };
                var expr = MakeWrappedExpression(wrapperType.Name, typeArgs, context);
                if (expr is not null)
                {
                    return ObjectCreationExpression(
                        expr,
                        argListFromMemberName,
                        initializer: null);
                }
            }

            // 2. A wrapper for a built-in primitive (non-generic type)

            var wrapperName = TryGetPrimitiveWrapper(type);
            if (wrapperName is not null)
            {
                return ObjectCreationExpression(
                    IdentifierName(wrapperName),
                    argListFromMemberName,
                    initializer: null);
            }

            // 3. A wrapper for a compound type (might need nested wrappers)

            var wrapperTypeSyntax = TryGetCompoundWrapper(type, context);
            if (wrapperTypeSyntax is not null)
            {
                return ObjectCreationExpression(
                    wrapperTypeSyntax,
                    argListFromMemberName,
                    initializer: null);
            }

            return null;
        }

        private static AttributeData? TryGetExplicitWrapper(DataMemberSymbol member)
        {
            foreach (var attr in member.Symbol.GetAttributes())
            {
                if (attr.AttributeClass?.Name is "SerdeWrapAttribute")
                {
                    return attr;
                }
            }
            return null;
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
            if (elemTypes.Length == 0)
            {
                return IdentifierName(baseWrapperName);
            }

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

        private static bool ImplementsISerialize(ITypeSymbol memberType, GeneratorExecutionContext context)
        {
            // Check if the type either has the GenerateSerialize attribute, or directly implements ISerialize
            // (If the type has the GenerateSerialize attribute then the generator will implement the interface)
            var attributes = memberType.GetAttributes();
            foreach (var attr in attributes)
            {
                if (attr.AttributeClass?.Name is "GenerateSerializeAttribute")
                {
                    return true;
                }
            }

            var iserializeSymbol = context.Compilation.GetTypeByMetadataName("Serde.ISerialize`4");
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