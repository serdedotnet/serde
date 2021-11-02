
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

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

        public static List<DataMemberSymbol> GetPublicDataMembers(ITypeSymbol type)
            => type.GetMembers()
            .Where(m => m is {
                DeclaredAccessibility: Accessibility.Public,
                Kind: SymbolKind.Field or SymbolKind.Property,
            })
            .Select(m => new DataMemberSymbol(m)).ToList();
    }
}