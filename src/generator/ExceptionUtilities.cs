
using System;

namespace Serde
{
    internal static class ExceptionUtilities
    {
        public static Exception Unreachable => new InvalidOperationException("This location is thought to be unreachable");

        public static Exception UnexpectedValue<T>(T t) => new InvalidOperationException($"Value {t} was unexpected");
    }
}