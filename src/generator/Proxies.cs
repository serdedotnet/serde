
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Collections.Generic;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Serde.Diagnostics;
using static Serde.WellKnownTypes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Reflection.Metadata;

namespace Serde;

/// <summary>
/// Holds the type and the proxy. This may be the original type passed in, or it may be a
/// converted type, if the proxy doesn't proxy the original type directly.
/// </summary>
internal readonly record struct TypeWithProxy(
    string Type,
    string Proxy);

partial class Proxies
{
    internal static string GetProxyName(string typeName) => typeName + "Proxy";

    internal static TypeDeclContext GenerateEnumProxy(
        TypeDeclContext typeDeclContext,
        GeneratorExecutionContext context)
    {
        // Generate enum wrapper stub
        var typeName = typeDeclContext.Name;
        var proxyName = GetProxyName(typeName);
        var newType = new SourceBuilder($$"""
sealed partial class {{proxyName}};
""")!;
        newType = typeDeclContext.MakeSiblingType(newType);
        var newDecl = TypeDeclContext.FromFile(newType.ToString(), proxyName);

        context.AddSource(newDecl.GetFqn(), newType);
        return newDecl;
    }

    internal static string? TryGetPrimitiveName(ITypeSymbol type)
    {
        // Nullable types are not considered primitive types
        if (type.NullableAnnotation == NullableAnnotation.Annotated)
        {
            return null;
        }
        var spTypeMatch = type.SpecialType switch
        {
            SpecialType.System_Boolean => "Bool",
            SpecialType.System_Byte => "U8",
            SpecialType.System_Char => "Char",
            SpecialType.System_Decimal => "Decimal",
            SpecialType.System_Double => "F64",
            SpecialType.System_Int16 => "I16",
            SpecialType.System_Int32 => "I32",
            SpecialType.System_Int64 => "I64",
            SpecialType.System_SByte => "I8",
            SpecialType.System_Single => "F32",
            SpecialType.System_String => "String",
            SpecialType.System_UInt16 => "U16",
            SpecialType.System_UInt32 => "U32",
            SpecialType.System_UInt64 => "U64",
            _ => null,
        };
        if (spTypeMatch is not null)
        {
            return spTypeMatch;
        }
        if (type is { Name: "DateTime",
                      ContainingNamespace: {
                        Name: "System",
                        ContainingNamespace: { IsGlobalNamespace: true } } })
        {
            return "DateTime";
        }
        return null;
    }

    // If the target is a core type, we can wrap it
    internal static TypeWithProxy? TryGetPrimitiveProxy(ITypeSymbol type)
    {
        var primNameProxy = TryGetPrimitiveName(type).Map<string, TypeWithProxy>(name =>
        {
            var proxy = GetProxyName(name);
            return new(name, $"global::Serde.{proxy}");
        });
        if (primNameProxy is not null)
        {
            return primNameProxy;
        }
        // These are types that have proxies, but not dedicated methods on ISerialize/IDeserialize
        return type switch
        {
            { Name: "Guid",
              ContainingNamespace: {
                Name: "System",
                ContainingNamespace: { IsGlobalNamespace: true } } }
            => new("global::System.Guid", "global::Serde.GuidProxy"),
            { Name: "DateTimeOffset",
              ContainingNamespace: {
                Name: "System",
                ContainingNamespace: { IsGlobalNamespace: true } } }
            => new("global::System.DateTimeOffset", "global::Serde.DateTimeOffsetProxy"),
            IArrayTypeSymbol { ElementType: { SpecialType: SpecialType.System_Byte } }
            => new("global::System.Byte[]", "global::Serde.ByteArrayProxy"),
            _ => null
        };
    }

    private static TypeWithProxy? TryGetCompoundWrapper(
        ITypeSymbol type,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        (string?, string?)? valueTypeAndProxy = type switch
        {
            { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } =>
                (type.ToDisplayString(s_fqnFormat), MakeProxyType(
                    $"Serde.NullableProxy.{GetSingletonImplName(usage)}",
                    ImmutableArray.Create(((INamedTypeSymbol)type).TypeArguments[0]),
                    context,
                    usage,
                    inProgress)),

            // This is rather subtle. One might think that we would want to use a
            // NullableRefWrapper for any reference type that could contain null. In fact, we
            // only want to use one if the type in question is actually annotated as nullable.
            // The difference comes down to type parameters. If a type parameter is constrained
            // as `class?` then it is both a reference type and nullable, but we don't want to
            // use a wrapper for it. The reason why is that we don't know the actual "underlying"
            // type and couldn't dispatch to the underlying type's ISerialize implementation.
            // Instead, for type parameters that aren't actually "annotated" as nullable (i.e.,
            // "T?") we must rely on the type parameter itself implementing ISerialize, and
            // therefore the substitution to provide the appropriate nullable wrapper.
            { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated} =>
                (type.ToDisplayString(s_fqnFormat), MakeProxyType(
                    $"Serde.NullableRefProxy.{GetSingletonImplName(usage)}",
                    ImmutableArray.Create(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)),
                    context,
                    usage,
                    inProgress)),

            IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType } =>
                (type.ToDisplayString(s_fqnFormat), MakeProxyType(
                    $"Serde.ArrayProxy.{GetSingletonImplName(usage)}",
                    ImmutableArray.Create(elemType),
                    context,
                    usage,
                    inProgress)),

            INamedTypeSymbol t when TryGetWrapperComponents(t, context, usage) is (var ConvertedType, var WrapperType, var Args)
                => (ConvertedType, MakeProxyType(WrapperType, Args, context, usage, inProgress)),

            _ => null,
        };
        return valueTypeAndProxy switch {
            null => null,
            ({ }, null) => null,
            ({ } value, { } wrapper) => new(value, wrapper),
            _ => throw ExceptionUtilities.Unreachable
        };
    }

    private static string GetSingletonImplName(SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "Ser",
        SerdeUsage.Deserialize => "De",
        _ => throw ExceptionUtilities.Unreachable
    };

    private static TypeWithProxy? TryGetEnumProxy(ITypeSymbol type, SerdeUsage usage)
    {
        if (type.TypeKind is not TypeKind.Enum)
        {
            return null;
        }

        // Check for the generation attributes
        if (!SerdeImplRoslynGenerator.HasGenerateAttribute(type, usage))
        {
            return null;
        }

        var typeName = type.ToDisplayString(s_fqnFormat);
        return new(typeName, GetProxyName(typeName));
    }

    private static string? MakeProxyType(
        string baseWrapperName,
        ImmutableArray<ITypeSymbol> elemTypes,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        if (elemTypes.Length == 0)
        {
            return baseWrapperName;
        }

        var typeArgs = new List<string>();
        foreach (var elemType in elemTypes)
        {
            var elemTypeSyntax = elemType.ToDisplayString(s_fqnFormat);
            typeArgs.Add(elemTypeSyntax);
        }

        foreach (var elemType in elemTypes)
        {
            // Check if the type directly implements the interface
            if (SerdeImplRoslynGenerator.ImplementsSerde(elemType, elemType, context, usage))
            {
                typeArgs.Add(elemType.ToDisplayString(s_fqnFormat));
                continue;
            }

            // Otherwise we'll need to wrap the element type as well e.g.,
            //      ArrayWrap<`elemType`, `elemTypeWrapper`>
            switch (TryGetImplicitWrapper(elemType, context, usage, inProgress))
            {
                case null:
                    return null;
                case var (_, wrapper):
                    typeArgs.Add(wrapper);
                    break;
            }
        }

        return $"{baseWrapperName}<{string.Join(", ", typeArgs.Select(x => x.ToString()))}>";
    }

    /// <summary>
    /// Implicit wrappers are:
    ///     1. Wrappers for primitive types
    ///     2. Wrappers for enums
    ///     3. Compound wrappers for primitive types (e.g. List, Dictionary)
    /// </summary>
    internal static TypeWithProxy? TryGetImplicitWrapper(
        ITypeSymbol elemType,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        // If we're in the process of generating a proxy type for the given underlying, return
        // the proxy type we're currently generating. This is to avoid infinite recursion.
        foreach (var (receiver, containing) in inProgress)
        {
            if (SymbolEqualityComparer.IncludeNullability.Equals(receiver, elemType))
            {
                return new(
                    elemType.ToDisplayString(s_fqnFormat),
                    containing.ToDisplayString(s_fqnFormat)
                );
            }
        }
        return TryGetPrimitiveProxy(elemType)
            ?? TryGetEnumProxy(elemType, usage)
            ?? TryGetCompoundWrapper(elemType, context, usage, inProgress);
    }

    private static readonly SymbolDisplayFormat s_baseNameFormat = new(
        SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.None);

    private static readonly SymbolDisplayFormat s_fqnFormat = new(
        SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions:
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes
            | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
            | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

    internal static string? TryGetExplicitWrapper(
        DataMemberSymbol member,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<(ITypeSymbol Receiver, ITypeSymbol Containing)> inProgress)
    {
        if (TryGetExplicitProxyType(member, usage, context) is {} proxyType)
        {
            var memberType = member.Type;
            if (proxyType is INamedTypeSymbol { IsGenericType: true } && proxyType.Equals(proxyType.OriginalDefinition, SymbolEqualityComparer.Default))
            {
                // If the wrapper type is an unconstructed generic type which we need to construct
                // with the appropriate wrappers for the member type's type arguments.
                var baseName = proxyType.ToDisplayString(s_baseNameFormat);
                var typeArgs = memberType switch
                {
                    INamedTypeSymbol n => n.TypeArguments,
                    _ => ImmutableArray<ITypeSymbol>.Empty
                };
                return MakeProxyType(baseName, typeArgs, context, usage, inProgress);
            }

            if (!SerdeImplRoslynGenerator.ImplementsSerde(proxyType, memberType, context, usage))
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_WrapperDoesntImplementInterface,
                    member.Symbol.Locations[0],
                    proxyType,
                    memberType,
                    usage.GetInterfaceName() + "Provider"));
            }

            return proxyType.ToDisplayString(s_fqnFormat);
        }
        return null;

        // <summary>
        // Looks to see if the given member or its type explicitly specifies a wrapper to use via
        // the SerdeType or Member options. If so, returns the symbol of the wrapper type.
        // </summary>
        static ITypeSymbol? TryGetExplicitProxyType(DataMemberSymbol member, SerdeUsage usage, GeneratorExecutionContext context)
        {
            // Look first for a wrapper attribute on the member being serialized, and then for a
            // wrapper attribute
            var typeToWrap = member.Type;
            return GetSerdeProxyType(member.Symbol, typeToWrap, usage, context)
                ?? GetSerdeProxyType(member.Type, typeToWrap, usage, context);

            static ITypeSymbol? GetSerdeProxyType(ISymbol symbol, ITypeSymbol typeToWrap, SerdeUsage usage, GeneratorExecutionContext context)
            {
                foreach (var attr in symbol.GetAttributes())
                {
                    if (attr is { AttributeClass.Name: nameof(SerdeMemberOptions) or nameof(SerdeTypeOptions), NamedArguments: { } named })
                    {
                        foreach (var arg in named)
                        {
                            switch (arg)
                            {
                                case { Key: nameof(SerdeTypeOptions.Proxy), Value.Value: INamedTypeSymbol proxyType }:
                                {
                                    // If the typeToWrap is a generic type, we should expect that the wrapper type
                                    // is not listed directly, but instead a parent type is listed (possibly static) and
                                    // the Serialize and Deserialize wrappers are nested below.
                                    if (typeToWrap.OriginalDefinition is INamedTypeSymbol { Arity: > 0 } targetType)
                                    {
                                        var nestedName = GetSingletonImplName(usage);
                                        var nestedTypes = proxyType.GetTypeMembers(nestedName);
                                        if (nestedTypes is not [ {} elem ])
                                        {
                                            context.ReportDiagnostic(CreateDiagnostic(
                                                DiagId.ERR_CantFindNestedWrapper,
                                                symbol.Locations[0],
                                                nestedName,
                                                proxyType,
                                                targetType));
                                            return null;
                                        }
                                        return elem;
                                    }
                                    return proxyType;
                                }
                                case
                                {
                                    Key: nameof(SerdeMemberOptions.SerializeProxy),
                                    Value.Value: INamedTypeSymbol wrapperType
                                } when usage.HasFlag(SerdeUsage.Serialize):
                                {
                                    return wrapperType;
                                }
                                case
                                {
                                    Key: nameof(SerdeMemberOptions.DeserializeProxy),
                                    Value.Value: INamedTypeSymbol wrapperType2
                                } when usage.HasFlag(SerdeUsage.Deserialize):
                                {
                                    return wrapperType2;
                                }

                                case
                                {
                                    Key: nameof(SerdeMemberOptions.TypeParameterProxy),
                                    Value.Value: string typeParamName
                                }:
                                {
                                    var typeParam = FindContainingTypeParamWithName(symbol, typeParamName);
                                    if (typeParam is not null)
                                    {
                                        return typeParam;
                                    }
                                    // If we didn't find the type parameter, report an error
                                    context.ReportDiagnostic(CreateDiagnostic(
                                        DiagId.ERR_CantFindTypeParameter,
                                        symbol.Locations[0],
                                        typeParamName));
                                    break;
                                }

                                case
                                {
                                    Key: nameof(SerdeMemberOptions.SerializeTypeParameterProxy),
                                    Value.Value: string typeParamName
                                } when usage.HasFlag(SerdeUsage.Serialize):
                                {
                                    var typeParam = FindContainingTypeParamWithName(symbol, typeParamName);
                                    if (typeParam is not null)
                                    {
                                        return typeParam;
                                    }
                                    // If we didn't find the type parameter, report an error
                                    context.ReportDiagnostic(CreateDiagnostic(
                                        DiagId.ERR_CantFindTypeParameter,
                                        symbol.Locations[0],
                                        typeParamName));
                                    break;
                                }

                                case
                                {
                                    Key: nameof(SerdeMemberOptions.DeserializeTypeParameterProxy),
                                    Value.Value: string typeParamName
                                } when usage.HasFlag(SerdeUsage.Deserialize):
                                {
                                    var typeParam = FindContainingTypeParamWithName(symbol, typeParamName);
                                    if (typeParam is not null)
                                    {
                                        return typeParam;
                                    }
                                    // If we didn't find the type parameter, report an error
                                    context.ReportDiagnostic(CreateDiagnostic(
                                        DiagId.ERR_CantFindTypeParameter,
                                        symbol.Locations[0],
                                        typeParamName));
                                    break;
                                }
                            }

                            // Walk up the containing types to find a type parameter with the given name.
                            static ITypeSymbol? FindContainingTypeParamWithName(ISymbol current, string name)
                            {
                                var containing = current.ContainingType;
                                while (containing is not null)
                                {
                                    var typeParams = containing.TypeParameters;
                                    foreach (var t in typeParams)
                                    {
                                        if (t.Name == name)
                                        {
                                            return t;
                                        }
                                    }
                                    containing = containing.ContainingType;
                                }
                                return null;
                            }
                        }
                    }
                }
                return null;
            }
        }
    }

    [return: NotNullIfNotNull(nameof(wkOpt))]
    internal static string? ToProxy(WellKnownType? wkOpt, Compilation comp, SerdeUsage usage)
    {
        if (wkOpt is not {} wk)
        {
            return null;
        }
        var typeName = wk switch
        {
            WellKnownType.ImmutableArray_1 => "ImmutableArrayProxy",
            WellKnownType.List_1 => "ListProxy",
            WellKnownType.Dictionary_2 => "DictProxy",
        };
        return $"Serde.{typeName}.{GetSingletonImplName(usage)}";
    }

    private static (string MemberType, string ProxyType, ImmutableArray<ITypeSymbol> Args)? TryGetWrapperComponents(
        ITypeSymbol typeSymbol,
        GeneratorExecutionContext context,
        SerdeUsage usage)
    {
        var typeString = typeSymbol.ToDisplayString(s_fqnFormat);
        if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
        {
            var nonNull = typeSymbol.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
            return (typeString, $"Serde.NullableRefProxy{GetSingletonImplName(usage)}", ImmutableArray.Create(nonNull));
        }

        if (typeSymbol is INamedTypeSymbol named && TryGetWellKnownType(named, context) is {} wk)
        {
            return (typeString, ToProxy(wk, context.Compilation, usage), named.TypeArguments);
        }

        return null;
    }
}

internal static class SerdeUsageExt
{
    public static string GetInterfaceName(this SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "ISerialize",
        SerdeUsage.Deserialize => "IDeserialize",
        SerdeUsage.Both => "ISerde",
        _ => throw ExceptionUtilities.Unreachable
    };

    public static string GetSerdeObjName(this SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "_SerObj",
        SerdeUsage.Deserialize => "_DeObj",
        SerdeUsage.Both => "_SerdeObj",
        _ => throw ExceptionUtilities.Unreachable
    };
}