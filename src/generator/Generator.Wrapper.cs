
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
    partial class SerdeImplRoslynGenerator
    {
        internal static void GenerateWrapper(
            GeneratorExecutionContext context,
            AttributeData attributeData,
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

            if (attributeData.ConstructorArguments is not [ { Value: string memberName } ])
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
                    attributeData.ApplicationSyntaxReference!.GetSyntax().GetLocation(),
                    receiverType));
                return;
            }

            var inProgress = ImmutableList.Create(receiverType);

            GenerateImpl(SerdeUsage.Serialize, new TypeDeclContext(typeDecl), receiverType, receiverExpr, context, inProgress);
            GenerateImpl(SerdeUsage.Deserialize, new TypeDeclContext(typeDecl), receiverType, receiverExpr, context, inProgress);
        }

        private static TypeSyntax? TryGetCompoundWrapper(ITypeSymbol type, GeneratorExecutionContext context, SerdeUsage usage, ImmutableList<ITypeSymbol> inProgress)
        {
            return type switch
            {
                { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } =>
                    MakeWrappedExpression(
                        $"NullableWrap.{usage.GetImplName()}",
                        ImmutableArray.Create(((INamedTypeSymbol)type).TypeArguments[0]),
                        context,
                        usage,
                        inProgress),

                // This is rather subtle. One might think that we would want to use a
                // NullableRefWrapper for any reference type that could contain null. In fact, we
                // only want to use one if the type in question is actually annotated as nullable.
                // The difference comes down to type parameters. If a type parameter is constrained
                // as `class?` then it is both a reference type and nullable, but we don't want to
                // use a wrapper for it. The reason why is that in we don't know the actual
                // "underlying" type and couldn't dispatch to the underlying type's ISerialize
                // implementation. Instead, for type parameters that aren't actually "annotated" as
                // nullable (i.e., "T?") we must rely on the type parameter itself implementing
                // ISerialize, and therefore the substitution to provide the appropriate nullable
                // wrapper.
                { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated} =>
                    MakeWrappedExpression(
                        $"NullableRefWrap.{usage.GetImplName()}",
                        ImmutableArray.Create(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)),
                        context,
                        usage,
                        inProgress),

                IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType }
                    => MakeWrappedExpression(
                        $"ArrayWrap.{usage.GetImplName()}",
                        ImmutableArray.Create(elemType),
                        context,
                        usage,
                        inProgress),

                INamedTypeSymbol t when TryGetWrapperName(t, context, usage) is { } tuple
                    => MakeWrappedExpression(tuple.WrapperName, tuple.Args, context, usage, inProgress),

                _ => null,
            };
        }

        private static TypeSyntax? MakeWrappedExpression(
            string baseWrapperName,
            ImmutableArray<ITypeSymbol> elemTypes,
            GeneratorExecutionContext context,
            SerdeUsage usage,
            ImmutableList<ITypeSymbol> inProgress)
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
                var wrapper = TryGetAnyWrapper(elemType, context, usage, inProgress);

                if (wrapper is null)
                {
                    // Could not find a wrapper
                    return null;
                }
                else
                {
                    wrapperTypes.Add(elemTypeSyntax);
                    wrapperTypes.Add(wrapper);
                }
            }

            return GenericName(
                Identifier(baseWrapperName), TypeArgumentList(SeparatedList(wrapperTypes)));
        }

        private static TypeSyntax? TryGetAnyWrapper(
            ITypeSymbol elemType,
            GeneratorExecutionContext context,
            SerdeUsage usage,
            ImmutableList<ITypeSymbol> inProgress)
        {
            // If we're in the process of generating a wrapper type, just return the name of the wrapper
            // and assume it has been generated already.
            if (inProgress.Contains(elemType, SymbolEqualityComparer.Default))
            {
                var typeName = elemType.Name;
                var allTypes = typeName;
                for (var parent = elemType.ContainingType; parent is not null; parent = parent.ContainingType)
                {
                    allTypes = parent.Name + allTypes;
                }
                var wrapperName = $"{allTypes}Wrap";
                return IdentifierName(wrapperName);
            }
            return TryGetPrimitiveWrapper(elemType, usage)
                ?? TryGetEnumWrapper(elemType, usage)
                ?? TryGetCompoundWrapper(elemType, context, usage, inProgress);
        }

        private static TypeSyntax? TryGetExplicitWrapper(
            DataMemberSymbol member,
            GeneratorExecutionContext context,
            SerdeUsage usage,
            ImmutableList<ITypeSymbol> inProgress)
        {
            if (TryGetExplicitWrapperType(member, usage, context) is {} wrapperType)
            {
                var memberType = member.Type;
                var typeArgs = memberType switch
                {
                    INamedTypeSymbol n => n.TypeArguments,
                    _ => ImmutableArray<ITypeSymbol>.Empty
                };
                var wrapName = wrapperType.ToString();
                if (wrapName.IndexOf('<') is > 0 and var index)
                {
                    wrapName = wrapName[..index];
                }
                return MakeWrappedExpression(wrapName, typeArgs, context, usage, inProgress);
            }
            return null;

            // <summary>
            // Looks to see if the given member or its type explicitly specifies a wrapper to use via
            // the SerdeWrapAttribute or similar. If so, returns the symbol of the wrapper type.
            // </summary>
            static INamedTypeSymbol? TryGetExplicitWrapperType(DataMemberSymbol member, SerdeUsage usage, GeneratorExecutionContext context)
            {
                // Look first for a wrapper attribute on the member being serialized, and then for a
                // wrapper attribute
                var typeToWrap = member.Type;
                return GetSerdeWrapAttributeArg(member.Symbol, typeToWrap, usage, context)
                    ?? GetSerdeWrapAttributeArg(member.Type, typeToWrap, usage, context);

                static INamedTypeSymbol? GetSerdeWrapAttributeArg(ISymbol symbol, ITypeSymbol typeToWrap, SerdeUsage usage, GeneratorExecutionContext context)
                {
                    foreach (var attr in symbol.GetAttributes())
                    {
                        if (attr.AttributeClass?.Name is "SerdeWrapAttribute")
                        {
                            if (attr is { ConstructorArguments: { Length: 1 } attrArgs } &&
                                attrArgs[0] is { Value: INamedTypeSymbol wrapperType })
                            {
                                // If the typeToWrap is a generic type, we should expect that the wrapper type
                                // is not listed directly, but instead a parent type is listed (possibly static) and
                                // the Serialize and Deserialize wrappers are nested below.
                                if (typeToWrap.OriginalDefinition is INamedTypeSymbol { Arity: > 0 } wrapped)
                                {
                                    var nestedName = (usage == SerdeUsage.Serialize ? "Serialize" : "Deserialize") + "Impl";
                                    var nestedTypes = wrapperType.GetTypeMembers(nestedName);
                                    if (nestedTypes.Length != 1)
                                    {
                                        context.ReportDiagnostic(CreateDiagnostic(
                                            DiagId.ERR_CantFindNestedWrapper,
                                            symbol.Locations[0],
                                            nestedName,
                                            wrapperType,
                                            wrapped));
                                        return null;
                                    }
                                    return nestedTypes[0];
                                }
                                return wrapperType;
                            }
                            // Return null if the attribute is somehow incorrect
                            // TODO: produce a warning?
                            return null;
                        }
                    }
                    return null;
                }
            }
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