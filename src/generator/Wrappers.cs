
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

namespace Serde;

/// <summary>
/// Holds the wrapped type and the wrapper. This may be the original type passed in, or it may be a
/// converted type, if the wrapper doesn't wrap the original type directly.
/// </summary>
internal readonly record struct TypeWithWrapper(
    TypeSyntax Type,
    TypeSyntax Wrapper);

partial class Wrappers
{
    internal static string GetWrapperName(string typeName) => typeName + "Wrap";

    internal static void GenerateEnumWrapper(
        BaseTypeDeclarationSyntax typeDecl,
        SemanticModel semanticModel,
        GeneratorExecutionContext context)
    {
        var receiverType = semanticModel.GetDeclaredSymbol(typeDecl);
        if (receiverType is null)
        {
            return;
        }

        // Generate enum wrapper stub
        var typeDeclContext = new TypeDeclContext(typeDecl);
        var typeName = typeDeclContext.Name;
        var wrapperName = GetWrapperName(typeName);
        var newType = SyntaxFactory.ParseMemberDeclaration($$"""
readonly partial struct {{wrapperName}} { }
""")!;
        newType = typeDeclContext.WrapNewType(newType);
        string fullWrapperName = string.Join(".", typeDeclContext.NamespaceNames
            .Concat(typeDeclContext.ParentTypeInfo.Select(x => x.Name))
            .Concat(new[] { wrapperName }));

        var tree = CompilationUnit(
            externs: default,
            usings: default,
            attributeLists: default,
            members: List<MemberDeclarationSyntax>(new[] { newType }));
        tree = tree.NormalizeWhitespace(eol: Utilities.NewLine);

        context.AddSource(fullWrapperName, Utilities.NewLine + tree.ToFullString());
    }

    // If the target is a core type, we can wrap it
    internal static TypeWithWrapper? TryGetPrimitiveWrapper(ITypeSymbol type, SerdeUsage usage)
    {
        if (type.NullableAnnotation == NullableAnnotation.Annotated)
        {
            return null;
        }
        var name = type.SpecialType switch
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
            SpecialType.System_Single => "FloatWrap",
            SpecialType.System_Double => "DoubleWrap",
            SpecialType.System_Decimal => "DecimalWrap",
            _ => null
        };
        return name is null ? null : new(type.ToFqnSyntax(), ParseTypeName("global::Serde." + name));
    }

    private static TypeWithWrapper? TryGetCompoundWrapper(
        ITypeSymbol type,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<ITypeSymbol> inProgress)
    {
        (TypeSyntax?, TypeSyntax?)? valueTypeAndWrapper = type switch
        {
            { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } =>
                (type.ToFqnSyntax(), MakeWrappedExpression(
                    $"Serde.NullableWrap.{GetImplName(usage)}",
                    ImmutableArray.Create(((INamedTypeSymbol)type).TypeArguments[0]),
                    context,
                    usage,
                    inProgress)),

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
                (type.ToFqnSyntax(), MakeWrappedExpression(
                    $"Serde.NullableRefWrap.{GetImplName(usage)}",
                    ImmutableArray.Create(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)),
                    context,
                    usage,
                    inProgress)),

            IArrayTypeSymbol and { IsSZArray: true, Rank: 1, ElementType: { } elemType } =>
                (type.ToFqnSyntax(), MakeWrappedExpression(
                    $"Serde.ArrayWrap.{GetImplName(usage)}",
                    ImmutableArray.Create(elemType),
                    context,
                    usage,
                    inProgress)),

            INamedTypeSymbol t when TryGetWrapperComponents(t, context, usage) is (var ConvertedType, var WrapperType, var Args)
                => (ConvertedType, MakeWrappedExpression(WrapperType, Args, context, usage, inProgress)),

            _ => null,
        };
        return valueTypeAndWrapper switch {
            null => null,
            ({ } value, { } wrapper) => new(value, wrapper),
            _ => throw ExceptionUtilities.Unreachable
        };
    }

    private static string GetImplName(SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "SerializeImpl",
        SerdeUsage.Deserialize => "DeserializeImpl",
        _ => throw ExceptionUtilities.Unreachable
    };

    private static TypeWithWrapper? TryGetEnumWrapper(ITypeSymbol type, SerdeUsage usage)
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

        var wrapperName = GetWrapperName(type.ToDisplayString(s_baseNameFormat));
        return new(type.ToFqnSyntax(), ParseTypeName(wrapperName));
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

            if (SerdeImplRoslynGenerator.ImplementsSerde(elemType, elemType, context, usage))
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
                    wrapperTypes.Add(ParseTypeName($"global::Serde.IdWrap<{elemTypeSyntax}>"));
                }
                else
                {
                    wrapperTypes.Add(elemTypeSyntax);
                }
                continue;
            }

            // Otherwise we'll need to wrap the element type as well e.g.,
            //      ArrayWrap<`elemType`, `elemTypeWrapper`>
            switch (TryGetImplicitWrapper(elemType, context, usage, inProgress))
            {
                case null:
                    return null;
                case var (_, wrapper):
                    wrapperTypes.Add(elemTypeSyntax);
                    wrapperTypes.Add(wrapper);
                    break;
            }
        }

        return GenericName(
            Identifier(baseWrapperName), TypeArgumentList(SeparatedList(wrapperTypes)));
    }

    /// <summary>
    /// Implicit wrappers are:
    ///     1. Wrappers for primitive types
    ///     2. Wrappers for enums
    ///     3. Compound wrappers for primitive types (e.g. List, Dictionary)
    /// </summary>
    internal static TypeWithWrapper? TryGetImplicitWrapper(
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
                allTypes = parent.Name + "." + allTypes;
            }
            var wrapperName = GetWrapperName(allTypes);
            return new(elemType.ToFqnSyntax(), IdentifierName(wrapperName));
        }
        return TryGetPrimitiveWrapper(elemType, usage)
            ?? TryGetEnumWrapper(elemType, usage)
            ?? TryGetCompoundWrapper(elemType, context, usage, inProgress);
    }

    private static readonly SymbolDisplayFormat s_baseNameFormat = new(
        SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.None);

    internal static TypeSyntax? TryGetExplicitWrapper(
        DataMemberSymbol member,
        GeneratorExecutionContext context,
        SerdeUsage usage,
        ImmutableList<ITypeSymbol> inProgress)
    {
        if (TryGetExplicitWrapperType(member, usage, context) is {} wrapperType)
        {
            var memberType = member.Type;
            if (wrapperType.IsGenericType && wrapperType.Equals(wrapperType.OriginalDefinition, SymbolEqualityComparer.Default))
            {
                // If the wrapper type is an unconstructed generic type which we need to construct
                // with the appropriate wrappers for the member type's type arguments.
                var baseName = wrapperType.ToDisplayString(s_baseNameFormat);
                var typeArgs = memberType switch
                {
                    INamedTypeSymbol n => n.TypeArguments,
                    _ => ImmutableArray<ITypeSymbol>.Empty
                };
                return MakeWrappedExpression(baseName, typeArgs, context, usage, inProgress);
            }

            if (!SerdeImplRoslynGenerator.ImplementsSerde(wrapperType, memberType, context, usage))
            {
                context.ReportDiagnostic(CreateDiagnostic(
                    DiagId.ERR_WrapperDoesntImplementInterface,
                    member.Symbol.Locations[0],
                    wrapperType,
                    memberType,
                    usage.GetInterfaceName()));
            }

            return ParseTypeName(wrapperType.ToDisplayString());
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
                                var nestedName = GetImplName(usage);
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
                    else if (attr is { AttributeClass.Name: nameof(SerdeMemberOptions), NamedArguments: { } named })
                    {
                        foreach (var arg in named)
                        {
                            if (usage.HasFlag(SerdeUsage.Serialize)
                                && arg is { Key: nameof(SerdeMemberOptions.WrapperSerialize),
                                            Value.Value: INamedTypeSymbol wrapperType })
                            {
                                return wrapperType;
                            }
                            if (usage.HasFlag(SerdeUsage.Deserialize)
                                && arg is { Key: nameof(SerdeMemberOptions.WrapperDeserialize),
                                            Value.Value: INamedTypeSymbol wrapperType2 })
                            {
                                return wrapperType2;
                            }
                        }
                    }
                }
                return null;
            }
        }
    }

    [return: NotNullIfNotNull(nameof(wkOpt))]
    internal static string? ToWrapper(WellKnownType? wkOpt, Compilation comp, SerdeUsage usage)
    {
        if (wkOpt is not {} wk)
        {
            return null;
        }
        var typeName = wk switch
        {
            WellKnownType.ImmutableArray_1 => "ImmutableArrayWrap",
            WellKnownType.List_1 => "ListWrap",
            WellKnownType.Dictionary_2 => "DictWrap",
        };
        return $"Serde.{typeName}.{GetImplName(usage)}";
    }

    private static (TypeSyntax MemberType, string WrapperType, ImmutableArray<ITypeSymbol> Args)? TryGetWrapperComponents(
        ITypeSymbol typeSymbol,
        GeneratorExecutionContext context,
        SerdeUsage usage)
    {
        var typeSyntax = typeSymbol.ToFqnSyntax();
        if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
        {
            var nonNull = typeSymbol.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
            return (typeSyntax, $"Serde.NullableRefWrap{GetImplName(usage)}", ImmutableArray.Create(nonNull));
        }

        if (typeSymbol is INamedTypeSymbol named && TryGetWellKnownType(named, context) is {} wk)
        {
            return (typeSyntax, ToWrapper(wk, context.Compilation, usage), named.TypeArguments);
        }

        return null;
    }
}

internal static class WrapUsageExtensions
{
    public static string GetInterfaceName(this SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "ISerialize",
        SerdeUsage.Deserialize => "IDeserialize",
        _ => throw ExceptionUtilities.Unreachable
    };

    public static string GetImplName(this SerdeUsage usage) => usage switch
    {
        SerdeUsage.Serialize => "SerializeImpl",
        SerdeUsage.Deserialize => "DeserializeImpl",
        _ => throw ExceptionUtilities.Unreachable
    };
}