
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serde
{
    internal static class SymbolUtilities
    {
        public static ITypeSymbol GetSymbolType(ISymbol symbol) => symbol switch
        {
            IFieldSymbol f => f.Type,
            IPropertySymbol p => p.Type,
            IParameterSymbol p => p.Type,
            ILocalSymbol l => l.Type,
            _ => throw new InvalidOperationException($"Unexpected symbol {symbol}")
        };

        public static List<DataMemberSymbol> GetDataMembers(ITypeSymbol type, SerdeUsage usage)
        {
            var members = new List<DataMemberSymbol>();
            var curType = type;
            while (curType is not ({ SpecialType: SpecialType.System_Object } or null))
            {
                var typeOptions = GetTypeOptions(curType);
                foreach (var m in curType.GetMembers())
                {
                    if (m is not {
                            DeclaredAccessibility: Accessibility.Public,
                            Kind: SymbolKind.Field or SymbolKind.Property })
                    {
                        continue;
                    }
                    if (members.FindIndex(m2 => m2.Symbol.Name == m.Name) != -1)
                    {
                        // If we already have a member with the same name, the derived type is
                        // hiding the base type member.
                        continue;
                    }
                    if (m is IPropertySymbol { Parameters: { Length: > 0 } })
                    {
                        // Skip indexers
                        continue;
                    }
                    if (curType.TypeKind != TypeKind.Enum && m.IsStatic)
                    {
                        continue;
                    }
                    var memberOptions = GetMemberOptions(m);
                    if (memberOptions.Skip ||
                        (memberOptions.SkipSerialize && usage == SerdeUsage.Serialize) ||
                        (memberOptions.SkipDeserialize && usage == SerdeUsage.Deserialize))
                    {
                        continue;
                    }
                    members.Add(new DataMemberSymbol(m, typeOptions, memberOptions));
                }
                curType = curType.BaseType;
            }
            return members;
        }

        internal static ImmutableArray<INamedTypeSymbol> GetDUTypeMembers(INamedTypeSymbol receiverType)
        {
            Debug.Assert(receiverType.IsAbstract);

            var builder = ImmutableArray.CreateBuilder<INamedTypeSymbol>();
            foreach (var t in receiverType.GetTypeMembers())
            {
                if (t.BaseType?.Equals(receiverType, SymbolEqualityComparer.Default) == true)
                {
                    builder.Add(t);
                }
            }
            return builder.ToImmutable();
        }

        public static TypeSyntax ToFqnSyntax(this ITypeSymbol t) => SyntaxFactory.ParseTypeName(t.ToDisplayString());

        public static MemberOptions GetMemberOptions(ISymbol member)
        {
            var options = new MemberOptions();
            foreach (var attr in member.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass is null)
                {
                    continue;
                }
                if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.SerdeMemberOptions))
                {
                    foreach (var named in attr.NamedArguments)
                    {
                        var value = named.Value.Value!;
                        options = named switch
                        {
                            {
                                Key: nameof(MemberOptions.ThrowIfMissing),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with { ThrowIfMissing = (bool)value },

                            {
                                Key: nameof(MemberOptions.Rename),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_String
                                }
                            } => options with { Rename = (string)value },

                            {
                                Key: nameof(MemberOptions.ProvideAttributes),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with { ProvideAttributes = (bool)value },

                            {
                                Key: nameof(MemberOptions.Skip),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with { Skip = (bool)value },

                            {
                                Key: nameof(MemberOptions.SkipSerialize),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with { SkipSerialize = (bool)value },

                            {
                                Key: nameof(MemberOptions.SkipDeserialize),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with { SkipDeserialize = (bool)value },

                            _ => options
                        };
                    }
                    break;
                }
            }
            return options;
        }

        internal static TypeOptions GetTypeOptions(ITypeSymbol type)
        {
            var options = new TypeOptions();
            foreach (var attr in type.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass is null)
                {
                    continue;
                }
                if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.SerdeTypeOptions))
                {
                    foreach ((string name, TypedConstant argument) in attr.NamedArguments)
                    {
                        var value = argument.Value;
                        if (value is null)
                        {
                            continue;
                        }
                        options = (name, argument) switch {
                            (nameof(TypeOptions.MemberFormat),
                                {
                                    Kind: TypedConstantKind.Enum,
                                    Type.Name: nameof(MemberFormat)
                                }) => options with { MemberFormat = (MemberFormat)value },
                            (nameof(TypeOptions.DenyUnknownMembers),
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            ) => options with { DenyUnknownMembers = (bool)value },
                            (nameof(TypeOptions.Rename),
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_String
                                }) => options with { Rename = (string)value },
                            _ => options
                        };
                    }
                    break;
                }
            }
            return options;
        }

        public static string ToFqn(this ITypeSymbol type) => type.ToDisplayString(FqnFormat);

        public static readonly SymbolDisplayFormat FqnFormat = new(
            SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
            SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            SymbolDisplayGenericsOptions.IncludeTypeParameters,
            miscellaneousOptions:
                SymbolDisplayMiscellaneousOptions.UseSpecialTypes
                | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
                | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);
    }
}