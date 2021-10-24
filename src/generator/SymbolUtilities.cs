
using System;
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
    }
}