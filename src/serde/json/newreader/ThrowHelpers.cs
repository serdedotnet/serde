
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Serde.IO;

namespace Serde.Json;

internal static class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ThrowIfEos(short s)
    {
        if (s == IByteReader.EndOfStream)
        {
            ThrowUnexpectedEndOfStream();
        }
        return (byte)s;
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowUnexpectedEndOfStream()
    {
        throw new JsonException("Unexpected end of stream");
    }

    public static JsonException GetUnexpectedEndOfStreamException()
    {
        return new JsonException("Unexpected end of stream");
    }

    public static byte SkipWhitespaceOrThrow<T>(this ref Utf8JsonLexer<T> reader)
        where T : IByteReader
    {
        var peek = reader.Peek();
        if (peek == IByteReader.EndOfStream)
        {
            ThrowUnexpectedEndOfStream();
        }
        return (byte)peek;
    }
}