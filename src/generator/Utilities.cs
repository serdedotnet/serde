
using System;
using System.Collections.Generic;

namespace Serde
{
    internal static class ExceptionUtilities
    {
        public static Exception Unreachable => new InvalidOperationException("This location is thought to be unreachable");

        public static Exception UnexpectedValue<T>(T t) => new InvalidOperationException($"Value {t} was unexpected");
    }

    internal static class Utilities
    {
        public static void Deconstruct<K, V>(this KeyValuePair<K, V> kvp, out K key, out V value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}