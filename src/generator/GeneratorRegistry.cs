
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Serde
{
    internal sealed class GeneratorRegistry
    {
        private readonly ConditionalWeakTable<Compilation, ConcurrentDictionary<string, ValueTuple>> _cwt =
            new ConditionalWeakTable<Compilation, ConcurrentDictionary<string, ValueTuple>>();

        /// <summary>
        /// Returns true if the type name was successfully registered, false if it was already present.
        /// </summary>
        public bool TryAdd(Compilation comp, string typeName)
        {
            var dict = _cwt.GetOrCreateValue(comp);
            return dict.TryAdd(typeName, new ValueTuple());
        }
    }
}