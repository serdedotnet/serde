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
            var receiverType = semanticModel.GetDeclaredSymbol(typeDecl);
            if (receiverType is null)
            {
                return;
            }
            var receiverExpr = ThisExpression();
            GenerateSerialize(context, typeDecl, semanticModel, receiverType, receiverExpr);
        }

        private void GenerateSerialize(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel semanticModel,
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

            var typeName = typeDecl.Identifier.ValueText;

            // Generate statements for ISerialize.Serialize implementation
            var statements = GenerateSerializeBody(context, typeDecl, semanticModel, receiverType, receiverExpr);

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
                    .WithModifiers(TokenList(Token(SyntaxKind.PartialKeyword)))
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
                for (var current = typeDecl.Parent; current is not null; current = current.Parent)
                {
                    switch (current)
                    {
                        case NamespaceDeclarationSyntax ns:
                            newType = ns.WithMembers(List(new[] { newType }));
                            break;
                        case TypeDeclarationSyntax parentType:
                            newType = parentType.WithMembers(List(new[] { newType }));
                            break;
                    }
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

                context.AddSource($"{fullTypeName}.ISerialize.cs",
                    Environment.NewLine + "#nullable enable" + Environment.NewLine + tree.ToFullString());
            }
        }

        private List<StatementSyntax>? GenerateSerializeBody(
            GeneratorExecutionContext context,
            TypeDeclarationSyntax typeDecl,
            SemanticModel model,
            ITypeSymbol receiverType,
            ExpressionSyntax receiverExpr)
        {
            var statements = new List<StatementSyntax>();

            var fieldsAndProps = SymbolUtilities.GetPublicDataMembers(receiverType);

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
                var memberExpr = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    receiverExpr,
                    IdentifierName(m.Name));

                // Always prefer a direct implementation of ISerialize
                if (ImplementsSerde(memberType, context, WrapUsage.Serialize))
                {
                    return memberExpr;
                }

                // Try using a built-in wrapper
                return TrySerializeBuiltIn(memberType, m, context, memberExpr);
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
            GeneratorExecutionContext context,
            ExpressionSyntax memberExpr)
        {
            var argListFromMemberName = ArgumentList(SeparatedList(new[] { Argument(memberExpr) }));
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
                var expr = MakeWrappedExpression(wrapperType.Name, typeArgs, context, WrapUsage.Serialize);
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

            var wrapperTypeSyntax = TryGetCompoundWrapper(type, context, WrapUsage.Serialize);
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

        private static TypeSyntax? TryGetCompoundWrapper(ITypeSymbol type, GeneratorExecutionContext context, WrapUsage usage)
        {
            switch (type)
            {
                case IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType }:
                    return MakeWrappedExpression($"ArrayWrap.{usage.GetName()}", ImmutableArray.Create(elemType), context, usage);

                case INamedTypeSymbol t:
                    if (TryGetWrapperName(t, context, usage) is {} tuple)
                    {
                        return MakeWrappedExpression(tuple.WrapperName, tuple.Args, context, usage);
                    }
                    break;
            }
            return null;
        }

        private static TypeSyntax? MakeWrappedExpression(
            string baseWrapperName,
            ImmutableArray<ITypeSymbol> elemTypes,
            GeneratorExecutionContext context,
            WrapUsage usage)
        {
            if (elemTypes.Length == 0)
            {
                return IdentifierName(baseWrapperName);
            }

            var wrapperTypes = new List<TypeSyntax>();
            foreach (var elemType in elemTypes)
            {
                var elemTypeSyntax = ParseTypeName(elemType.ToDisplayString());

                if (ImplementsSerde(elemType, context, usage))
                {
                    // Special case for List-like types:
                    // If the element type directly implements ISerialize, we can
                    // use a single-arity version of the wrapper
                    //      ArrayWrap<`elemType`>
                    wrapperTypes.Add(elemTypeSyntax);

                    // Otherwise we need an `IdWrap` which just delegates to the inner
                    // type.
                    //if (elemTypes.Length > 1)
                    if (usage == WrapUsage.Serialize)
                    {
                        wrapperTypes.Add(GenericName(
                            Identifier("IdWrap"), TypeArgumentList(SeparatedList(new TypeSyntax[] {
                                elemTypeSyntax
                            }))
                        ));
                    }
                    else
                    {
                        wrapperTypes.Add(elemTypeSyntax);
                    }
                    continue;
                }

                // Otherwise we'll need to wrap the element type as well e.g.,
                //      ArrayWrap<`elemType`, `elemTypeWrapper`>

                var primWrapper = TryGetPrimitiveWrapper(elemType);
                TypeSyntax? wrapperName = primWrapper is not null
                    ? IdentifierName(primWrapper)
                    : TryGetCompoundWrapper(elemType, context, usage);

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

        private static (string WrapperName, ImmutableArray<ITypeSymbol> Args)? TryGetWrapperName(
            ITypeSymbol typeSymbol,
            GeneratorExecutionContext context,
            WrapUsage usage)
        {
            if (typeSymbol is INamedTypeSymbol named && TryGetWellKnownType(named, context) is {} wk)
            {
                return (wk.ToWrapper(usage), named.TypeArguments);
            }
            // Check if it implements well-known interfaces
            foreach (var iface in WellKnownTypes.GetAvailableInterfacesInOrder(context))
            {
                Debug.Assert(iface.TypeKind == TypeKind.Interface);
                foreach (var impl in typeSymbol.Interfaces)
                {
                    if (impl.OriginalDefinition.Equals(iface, SymbolEqualityComparer.Default) &&
                        WellKnownTypes.TryGetWellKnownType(iface, context)?.ToWrapper(usage) is { } wrap)
                    {
                        return (wrap, impl.TypeArguments);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Check to see if the type implements ISerialize or IDeserialize, depending on the WrapUsage.
        /// </summary>
        private static bool ImplementsSerde(ITypeSymbol memberType, GeneratorExecutionContext context, WrapUsage usage)
        {
            // Check if the type either has the GenerateSerialize attribute, or directly implements ISerialize
            // (If the type has the GenerateSerialize attribute then the generator will implement the interface)
            var attributes = memberType.GetAttributes();
            foreach (var attr in attributes)
            {
                if (usage == WrapUsage.Serialize && attr.AttributeClass?.Name is "GenerateSerializeAttribute" ||
                    usage == WrapUsage.Deserialize && attr.AttributeClass?.Name is "GenerateDeserializeAttribute")
                {
                    return true;
                }
            }

            var serdeSymbol = context.Compilation.GetTypeByMetadataName(usage == WrapUsage.Serialize
                ? "Serde.ISerialize`4"
                : "Serde.IDeserialize`1");
            if (serdeSymbol is not null && memberType.Interfaces.Contains(serdeSymbol, SymbolEqualityComparer.Default))
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