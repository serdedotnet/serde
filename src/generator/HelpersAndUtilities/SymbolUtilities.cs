using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Serde.Diagnostics;

namespace Serde
{
    internal static class SymbolUtilities
    {
        public static ITypeSymbol GetSymbolType(ISymbol symbol) =>
            symbol switch
            {
                IFieldSymbol f => f.Type,
                IPropertySymbol p => p.Type,
                IParameterSymbol p => p.Type,
                ILocalSymbol l => l.Type,
                _ => throw new InvalidOperationException($"Unexpected symbol {symbol}"),
            };

        public static List<DataMemberSymbol> GetDataMembers(
            ITypeSymbol type,
            SerdeUsage usage,
            GeneratorExecutionContext context
        )
        {
            var members = new List<DataMemberSymbol>();
            var curType = type;
            while (curType is not ({ SpecialType: SpecialType.System_Object } or null))
            {
                var typeOptions = GetTypeOptions(curType);
                foreach (var m in curType.GetMembers())
                {
                    if (m is not { Kind: SymbolKind.Field or SymbolKind.Property })
                    {
                        continue;
                    }
                    if (m is IPropertySymbol { Parameters: { Length: > 0 } })
                    {
                        // Skip indexers
                        CheckOrdinal(m);
                        continue;
                    }
                    if (m.DeclaredAccessibility != Accessibility.Public)
                    {
                        // A non-public member is never serialized or deserialized. Report an
                        // explicit Ordinal on such a member, since it would otherwise be silently
                        // ignored.
                        CheckOrdinal(m);
                        continue;
                    }
                    if (members.FindIndex(m2 => m2.Symbol.Name == m.Name) != -1)
                    {
                        // If we already have a member with the same name, the derived type is
                        // hiding the base type member.
                        CheckOrdinal(m);
                        continue;
                    }
                    if (curType.TypeKind != TypeKind.Enum && m.IsStatic)
                    {
                        CheckOrdinal(m);
                        continue;
                    }
                    var memberOptions = GetMemberOptions(m);
                    if (
                        memberOptions.Skip
                        || (memberOptions.SkipSerialize && usage == SerdeUsage.Serialize)
                        || (memberOptions.SkipDeserialize && usage == SerdeUsage.Deserialize)
                    )
                    {
                        CheckOrdinal(m);
                        continue;
                    }
                    members.Add(new DataMemberSymbol(m, typeOptions, memberOptions));
                }
                curType = curType.BaseType;
            }

            if (type.TypeKind == TypeKind.Enum)
            {
                // Enum cases are mapped by position to their declared value, so an explicit ordinal
                // is meaningless on them and is rejected.
                foreach (var m in members)
                {
                    if (m.Ordinal is not null)
                    {
                        context?.ReportDiagnostic(
                            CreateDiagnostic(DiagId.ERR_OrdinalOnEnumMember, m.Locations[0], m.Name)
                        );
                    }
                }
            }
            else
            {
                members = ReorderByExplicitOrdinal(members, context);
            }
            return members;

            void CheckOrdinal(ISymbol m)
            {
                if (GetMemberOptions(m).Ordinal is not null)
                {
                    context.ReportDiagnostic(
                        CreateDiagnostic(DiagId.ERR_OrdinalOnSkippedMember, m.Locations[0], m.Name)
                    );
                }
            }
        }

        /// <summary>
        /// Reorders members by their explicit <see cref="DataMemberSymbol.Ordinal"/>, ascending. The
        /// resulting position is the physical field index used by the generated <c>ISerdeInfo</c> and
        /// the serialize/deserialize code, while the ordinal itself is preserved as the field's
        /// logical identity (see <c>ISerdeInfo.GetFieldOrdinal</c>).
        ///
        /// When any member specifies an explicit ordinal, every member must specify one; otherwise a
        /// diagnostic is reported. Ordinals must be non-negative and unique, but they need not be
        /// contiguous: gaps ("holes") are allowed.
        /// </summary>
        private static List<DataMemberSymbol> ReorderByExplicitOrdinal(
            List<DataMemberSymbol> members,
            GeneratorExecutionContext context
        )
        {
            var anyExplicit = false;
            var allExplicit = true;
            foreach (var m in members)
            {
                if (m.Ordinal is null)
                {
                    allExplicit = false;
                }
                else
                {
                    anyExplicit = true;
                }
            }

            if (!anyExplicit)
            {
                return members;
            }

            if (!allExplicit)
            {
                // When any member specifies an explicit ordinal, all members must specify one.
                foreach (var m in members)
                {
                    if (m.Ordinal is null)
                    {
                        context.ReportDiagnostic(
                            CreateDiagnostic(
                                DiagId.ERR_PartialMemberOrdinal,
                                m.Locations[0],
                                m.Name
                            )
                        );
                    }
                }
            }

            // Detect duplicate ordinals. Gaps between ordinals are allowed and are preserved as
            // holes in the logical ordinal space; only physical packing stays dense.
            var byOrdinal = new Dictionary<int, DataMemberSymbol>();
            foreach (var m in members)
            {
                if (m.Ordinal is not int ord)
                {
                    continue;
                }
                if (byOrdinal.TryGetValue(ord, out var existing))
                {
                    context.ReportDiagnostic(
                        CreateDiagnostic(
                            DiagId.ERR_DuplicateOrdinal,
                            m.Locations[0],
                            ord,
                            existing.Name,
                            m.Name
                        )
                    );
                }
                else
                {
                    byOrdinal[ord] = m;
                }
            }

            // Stable ascending sort by ordinal. Members without an explicit ordinal only occur in the
            // partial-ordinal error path; sort them last so the result is still deterministic.
            return members
                .Select((m, pos) => (m, pos))
                .OrderBy(t => t.m.Ordinal ?? int.MaxValue)
                .ThenBy(t => t.pos)
                .Select(t => t.m)
                .ToList();
        }

        internal static ImmutableArray<INamedTypeSymbol> GetDUTypeMembers(
            INamedTypeSymbol receiverType
        )
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

        public static TypeSyntax ToFqnSyntax(this ITypeSymbol t) =>
            SyntaxFactory.ParseTypeName(t.ToDisplayString());

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
                if (
                    WellKnownTypes.IsWellKnownAttribute(
                        attrClass,
                        WellKnownAttribute.SerdeMemberOptions
                    )
                )
                {
                    foreach (var named in attr.NamedArguments)
                    {
                        var value = named.Value.Value!;
                        options = named switch
                        {
                            {
                                Key: nameof(MemberOptions.ThrowIfMissing),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with
                            {
                                ThrowIfMissing = (bool)value,
                            },

                            {
                                Key: nameof(MemberOptions.Rename),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_String
                                }
                            } => options with
                            {
                                Rename = (string)value,
                            },

                            {
                                Key: nameof(MemberOptions.Ordinal),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Int32
                                }
                            } => options with
                            {
                                Ordinal = (int)value is var ord && ord >= 0 ? ord : null,
                            },

                            {
                                Key: nameof(MemberOptions.ProvideAttributes),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with
                            {
                                ProvideAttributes = (bool)value,
                            },

                            {
                                Key: nameof(MemberOptions.SerializeNull),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with
                            {
                                SerializeNull = (bool)value,
                            },

                            {
                                Key: nameof(MemberOptions.Skip),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with
                            {
                                Skip = (bool)value,
                            },

                            {
                                Key: nameof(MemberOptions.SkipSerialize),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with
                            {
                                SkipSerialize = (bool)value,
                            },

                            {
                                Key: nameof(MemberOptions.SkipDeserialize),
                                Value:
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            } => options with
                            {
                                SkipDeserialize = (bool)value,
                            },

                            _ => options,
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
                if (
                    WellKnownTypes.IsWellKnownAttribute(
                        attrClass,
                        WellKnownAttribute.SerdeTypeOptions
                    )
                )
                {
                    foreach ((string name, TypedConstant argument) in attr.NamedArguments)
                    {
                        var value = argument.Value;
                        if (value is null)
                        {
                            continue;
                        }
                        options = (name, argument) switch
                        {
                            (
                                nameof(TypeOptions.MemberFormat),
                                { Kind: TypedConstantKind.Enum, Type.Name: nameof(MemberFormat) }
                            ) => options with
                            {
                                MemberFormat = (MemberFormat)value,
                            },
                            (
                                nameof(TypeOptions.DenyUnknownMembers),
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            ) => options with
                            {
                                DenyUnknownMembers = (bool)value,
                            },
                            (
                                nameof(TypeOptions.AllowDuplicateKeys),
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_Boolean
                                }
                            ) => options with
                            {
                                AllowDuplicateKeys = (bool)value,
                            },
                            (
                                nameof(TypeOptions.Rename),
                                {
                                    Kind: TypedConstantKind.Primitive,
                                    Type.SpecialType: SpecialType.System_String
                                }
                            ) => options with
                            {
                                Rename = (string)value,
                            },
                            _ => options,
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
            SymbolDisplayMemberOptions.IncludeContainingType,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
                | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
                | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
        );
    }
}
