// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.CodeAnalysis;

namespace Serde
{
    internal enum WellKnownType
    {
        ImmutableArray_1,
        List_1
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

        private static WellKnownType? NameToWellKnownType(string s) => s switch
        {
            "ImmutableArray`1" => WellKnownType.ImmutableArray_1,
            "List`1" => WellKnownType.List_1,
             _ => null
        };

        private static string GetFQN(this WellKnownType wk) => wk switch
        {
            WellKnownType.ImmutableArray_1 => "System.Collections.Immutable.ImmutableArray`1",
            WellKnownType.List_1 => "System.Collections.Generic.List`1",
            _ => throw ExceptionUtilities.Unreachable
        };
    }
}