// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using StaticCs;

namespace Serde
{
    [Closed]
    internal enum WellKnownType
    {
        ImmutableArray_1,
        List_1,
        Dictionary_2,
    }

    [Closed]
    internal enum WellKnownAttribute
    {
        GenerateSerialize,
        GenerateDeserialize,
        GenerateSerde,
        SerdeTypeOptions,
        SerdeMemberOptions
    }

    internal static class WellKnownTypes
    {
        public static WellKnownType? TryGetWellKnownType(INamedTypeSymbol t, GeneratorExecutionContext context)
        {
            if (NameToWellKnownType(t.MetadataName) is {} wk)
            {
                var ia = context.Compilation.GetTypeByMetadataName(GetFQN(wk));
                if (ia is not null && t.OriginalDefinition.Equals(ia, SymbolEqualityComparer.Default))
                {
                    return wk;
                }
            }
            return null;
        }

        internal static string GetName(this WellKnownAttribute wk) => wk switch
        {
            WellKnownAttribute.GenerateDeserialize => nameof(WellKnownAttribute.GenerateDeserialize),
            WellKnownAttribute.GenerateSerialize => nameof(WellKnownAttribute.GenerateSerialize),
            WellKnownAttribute.GenerateSerde => nameof(WellKnownAttribute.GenerateSerde),
            WellKnownAttribute.SerdeTypeOptions => nameof(WellKnownAttribute.SerdeTypeOptions),
            WellKnownAttribute.SerdeMemberOptions => nameof(WellKnownAttribute.SerdeMemberOptions),
        };

        internal static bool HasMatchingName(string name, WellKnownAttribute wk)
        {
            var typeName = wk.GetName();
            return name.Equals(typeName, StringComparison.Ordinal) ||
                name.Equals(wk.GetFqn(), StringComparison.Ordinal);
        }

        internal static string GetFqn(this WellKnownAttribute wk) => "Serde." + wk.GetName();

        internal static bool IsWellKnownAttribute(INamedTypeSymbol type, WellKnownAttribute wk)
            => type.ToDisplayString().Equals(wk.GetFqn(), StringComparison.Ordinal);

        private static WellKnownType? NameToWellKnownType(string s) => s switch
        {
            "ImmutableArray`1" => WellKnownType.ImmutableArray_1,
            "List`1" => WellKnownType.List_1,
            "Dictionary`2" => WellKnownType.Dictionary_2,
             _ => null
        };

        private static string GetFQN(this WellKnownType wk) => wk switch
        {
            WellKnownType.ImmutableArray_1 => "System.Collections.Immutable.ImmutableArray`1",
            WellKnownType.List_1 => "System.Collections.Generic.List`1",
            WellKnownType.Dictionary_2 => "System.Collections.Generic.Dictionary`2",
            _ => throw ExceptionUtilities.Unreachable
        };

        internal static string ToWrapper(this WellKnownType wk, SerdeUsage usage)
        {
            var baseName = wk switch
            {
                WellKnownType.ImmutableArray_1 => "ImmutableArrayWrap",
                WellKnownType.List_1 => "ListWrap",
                WellKnownType.Dictionary_2 => "DictWrap",
                _ => throw ExceptionUtilities.Unreachable
            };
            return baseName + "." + usage.GetImplName();
        }
    }
}