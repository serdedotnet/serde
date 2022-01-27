
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.WellKnownTypes;

namespace Serde
{
    partial class Generator
    {
        internal void GenerateWrapper(
            GeneratorExecutionContext context,
            AttributeSyntax attributeSyntax,
            TypeDeclarationSyntax typeDecl,
            SemanticModel model)
        {
            // Type must be partial
            if (!typeDecl.Modifiers.Any(tok => tok.IsKind(SyntaxKind.PartialKeyword)))
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_TypeNotPartial,
                    typeDecl.Identifier.GetLocation(),
                    typeDecl.Identifier.ValueText));
                return;
            }

            var attrArgSyntax = attributeSyntax.ArgumentList?.Arguments[0];
            if (attrArgSyntax is null)
            {
                return;
            }
            var argValue = model.GetConstantValue(attrArgSyntax.Expression);
            if (argValue.Value is not string memberName)
            {
                return;
            }

            var containerSymbol = model.GetDeclaredSymbol(typeDecl)!;
            ExpressionSyntax receiverExpr;
            ITypeSymbol receiverType;
            var members = model.LookupSymbols(typeDecl.SpanStart, containerSymbol, memberName);
            if (members.Length != 1)
            {
                // Error about bad lookup
                return;
            }
            receiverType = SymbolUtilities.GetSymbolType(members[0]);
            receiverExpr = IdentifierName(memberName);

            if (receiverType.SpecialType != SpecialType.None)
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_CantWrapSpecialType,
                    attrArgSyntax.GetLocation(),
                    receiverType));
                return;
            }

            GenerateImpl(SerdeUsage.Serialize, new TypeDeclContext(typeDecl), receiverType, receiverExpr, context);
            GenerateImpl(SerdeUsage.Deserialize, new TypeDeclContext(typeDecl), receiverType, receiverExpr, context);
        }

        private static TypeSyntax? TryGetCompoundWrapper(ITypeSymbol type, GeneratorExecutionContext context, SerdeUsage usage)
        {
            switch (type)
            {
                case { NullableAnnotation: NullableAnnotation.Annotated }:
                    return MakeWrappedExpression(
                        $"NullableRefWrap.{usage.GetName()}",
                        ImmutableArray.Create(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)),
                        context,
                        usage);
                case IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType }:
                    return MakeWrappedExpression($"ArrayWrap.{usage.GetName()}", ImmutableArray.Create(elemType), context, usage);

                case INamedTypeSymbol t when TryGetWrapperName(t, context, usage) is {} tuple:
                    return MakeWrappedExpression(tuple.WrapperName, tuple.Args, context, usage);
            }
            return null;
        }

        private static TypeSyntax? MakeWrappedExpression(
            string baseWrapperName,
            ImmutableArray<ITypeSymbol> elemTypes,
            GeneratorExecutionContext context,
            SerdeUsage usage)
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
                    if (usage == SerdeUsage.Serialize)
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

                var primWrapper = TryGetPrimitiveWrapper(elemType, usage);
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
            SerdeUsage usage)
        {
            if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
            {
                return ("NullableRefWrap", ImmutableArray.Create(typeSymbol.WithNullableAnnotation(NullableAnnotation.NotAnnotated)));
            }
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

    }
}