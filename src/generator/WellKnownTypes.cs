// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Serde
{
    internal enum WellKnownType
    {
        ImmutableArray_1,
        List_1,
        Dictionary_2,
        IDictionary_2,
        IReadOnlyDictionary_2,
    }

    internal enum WellKnownAttribute
    {
        GenerateSerialize,
        GenerateDeserialize,
        GenerateSerde,
        GenerateWrapper,
        SerdeOptions
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

        public static ImmutableArray<INamedTypeSymbol> GetAvailableInterfacesInOrder(GeneratorExecutionContext context)
        {
            var builder = ImmutableArray.CreateBuilder<INamedTypeSymbol>();
            // Order matters here -- checking for interface implementation happens in order and
            // bails out early on the first success
            var candidates = new[] {
                WellKnownType.IDictionary_2,
                WellKnownType.IReadOnlyDictionary_2
            };
            foreach (var candidate in candidates)
            {
                if (context.Compilation.GetTypeByMetadataName(candidate.GetFQN()) is {} type)
                {
                    builder.Add(type);
                }
            }
            return builder.ToImmutable();
        }

        internal static string GetName(this WellKnownAttribute wk) => wk switch
        {
            WellKnownAttribute.GenerateDeserialize => "GenerateDeserialize",
            WellKnownAttribute.GenerateSerialize => "GenerateSerialize",
            WellKnownAttribute.GenerateSerde => "GenerateSerde",
            WellKnownAttribute.GenerateWrapper => "GenerateWrapper",
            WellKnownAttribute.SerdeOptions => "SerdeOptions",
            _ => throw ExceptionUtilities.UnexpectedValue(wk)
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
            "IDictionary`2" => WellKnownType.IDictionary_2,
            "IReadOnlyDictionary`2" => WellKnownType.IReadOnlyDictionary_2,
             _ => null
        };

        private static string GetFQN(this WellKnownType wk) => wk switch
        {
            WellKnownType.ImmutableArray_1 => "System.Collections.Immutable.ImmutableArray`1",
            WellKnownType.List_1 => "System.Collections.Generic.List`1",
            WellKnownType.Dictionary_2 => "System.Collections.Generic.Dictionary`2",
            WellKnownType.IDictionary_2 => "System.Collections.Generic.IDictionary`2",
            WellKnownType.IReadOnlyDictionary_2 => "System.Collections.Generic.IReadOnlyDictionary`2",
            _ => throw ExceptionUtilities.Unreachable
        };

        internal static string ToWrapper(this WellKnownType wk, SerdeUsage usage) => wk switch
        {
            WellKnownType.ImmutableArray_1 => "ImmutableArrayWrap",
            WellKnownType.List_1 => "ListWrap",
            WellKnownType.Dictionary_2 => "DictWrap",
            WellKnownType.IDictionary_2 => "IDictWrap",
            WellKnownType.IReadOnlyDictionary_2 => "IRODictWrap",
            _ => throw ExceptionUtilities.Unreachable
        } + "." + usage.GetName();
    }
}