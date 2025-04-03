
using System;
using System.Collections.Generic;

internal static class Utils
{
    public static void AssertEq<T>(T expected, T actual)
        where T : IEquatable<T>
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new Exception($"Expected {expected} but got {actual}");
        }
    }
}