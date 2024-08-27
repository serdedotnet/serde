
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serde
{
    internal static class ExceptionUtilities
    {
        public static Exception Unreachable => new InvalidOperationException("This location is thought to be unreachable");

        public static Exception UnexpectedValue<T>(T t) => new InvalidOperationException($"Value {t} was unexpected");
    }

    internal static class SpanExtensions
    {
        public static bool IsSorted<T>(this ReadOnlySpan<T> span) where T : IComparable<T>
            => IsSorted(span, Comparer<T>.Default);
        public static bool IsSorted<T>(this ReadOnlySpan<T> span, IComparer<T> comparer)
        {
            for (int i = 1; i < span.Length; i++)
            {
                if (comparer.Compare(span[i - 1], span[i]) > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }

    internal static class Utilities
    {
#pragma warning disable RS1035
        public static readonly string NewLine = Environment.NewLine;
#pragma warning restore RS1035

        public static T NotNull<T>(this T? value) where T : struct => value!.Value;

        public static string Concat(this string recv, string other)
        {
            return recv + other;
        }

        public static void Deconstruct<K, V>(this KeyValuePair<K, V> kvp, out K key, out V value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }

        public static IEnumerable<U> SelectNotNull<T, U>(this IEnumerable<T> en, Func<T, U?> f) where U : class
        {
            foreach (var item in en.Select(f))
            {
                if (item is not null)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<U> SelectNotNull<T, U>(this IEnumerable<T> en, Func<T, U?> f) where U : struct
        {
            foreach (var item in en.Select(f))
            {
                if (item is U u)
                {
                    yield return u;
                }
            }
        }
    }
}