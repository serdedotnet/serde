
using System;

namespace Serde
{
    internal static class ExceptionUtilities
    {
        public static Exception Unreachable => new InvalidOperationException("This location is thought to be unreachable");
    }
}