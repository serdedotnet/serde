
using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Serde.IO;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

internal struct Utf8JsonLexer<TReader>(TReader byteReader)
    where TReader : IByteReader
{
    public bool StartsWith(Utf8Span value) => byteReader.StartsWith(value);

    public bool GetBoolean()
    {
        bool result;
        switch (ThrowIfEos(SkipWhitespace()))
        {
            case (byte)'t' when byteReader.StartsWith("true"u8):
                result = true;
                Advance(4);
                break;
            case (byte)'f' when byteReader.StartsWith("false"u8):
                result = false;
                Advance(5);
                break;
            default:
                throw new InvalidOperationException("Expected boolean value");
        }
        return result;
    }

    public decimal GetDecimal(ScratchBuffer scratch)
    {
        CheckNumber();
        var span = LexNumber(skipOnly: false, scratch);
        if (!Utf8Parser.TryParse(span, out decimal result, out var bytesConsumed)
            || span.Length != bytesConsumed)
        {
            throw new InvalidOperationException("Invalid decimal value");
        }
        return result;
    }

    public double GetDouble(ScratchBuffer scratch)
    {
        CheckNumber();
        var span = LexNumber(skipOnly: false, scratch);
        if (!Utf8Parser.TryParse(span, out double result, out var bytesConsumed)
            || span.Length != bytesConsumed)
        {
            throw new InvalidOperationException("Invalid double value");
        }
        return result;
    }

    private void CheckNumber()
    {
        var peek = Peek();
        if (peek is not (byte)'-' and not (>= (byte)'0' and <= (byte)'9'))
        {
            throw new JsonException("Expected number, found: " + (char)peek);
        }
    }

    public long GetInt64(ScratchBuffer scratch) => (long)GetDecimal(scratch);

    public ulong GetUInt64(ScratchBuffer scratch) => (ulong)GetDecimal(scratch);

    public void Skip()
    {
        var peek = SkipWhitespace() switch {
            IByteReader.EndOfStream => throw new InvalidOperationException("Unexpected end of stream"),
            var s => (byte)s
        };
        switch (peek)
        {
            case (byte)'{':
                Advance();
                SkipObject();
                break;
            case (byte)'[':
                Advance();
                SkipArray();
                break;
            case (byte)'t' when byteReader.StartsWith("true"u8):
                Advance(4);
                break;
            case (byte)'f' when byteReader.StartsWith("false"u8):
                Advance(5);
                break;
            case (byte)'n' when byteReader.StartsWith("null"u8):
                Advance(4);
                break;
            case (byte)'"':
                Advance();
                _ = byteReader.LexUtf8Span(skipOnly: true, null);
                break;
            case (byte)'-' or (>= (byte)'0' and <= (byte)'9'):
                LexNumber(skipOnly: true, null);
                break;
            default:
                throw new JsonException("Unexpected char: " + (char)peek);
        }
    }

    /// <summary>
    ///  Assumes we are one byte past the '{' character.
    /// </summary>
    private void SkipObject()
    {
        // Objects contain zero or more key-value pairs, separated by commas
        // The pairs are separated by a colon
        var first = true;
        while (true)
        {
            var peek = SkipWhitespace();

            // Comma, Key, or End
            if (peek == (short)',')
            {
                if (first)
                {
                    throw new InvalidOperationException("Unexpected comma");
                }
                Advance();
                peek = SkipWhitespace();
            }

            // Key
            switch (peek)
            {
                case IByteReader.EndOfStream:
                    throw new InvalidOperationException("Unexpected end of stream");
                case (short)'}':
                    Advance();
                    return;
                default:
                    // Assume we are at the start of a key and let Skip handle it
                    Skip();
                    break;
            }

            // ':'
            peek = ThrowIfEos(SkipWhitespace());
            switch (peek)
            {
                case (short)':':
                    Advance();
                    break;
                default:
                    throw new JsonException($"Expected ':', found {(char)peek}");
            }

            // Value
            Skip();
            first = false;
        }
    }

    /// <summary>
    /// Assumes we are one byte past the '[' character.
    /// </summary>
    private void SkipArray()
    {
        bool first = true;
        while (true)
        {
            // Arrays contain zero or more values, separated by commas
            var peek = SkipWhitespace();
            if (peek == (short)',')
            {
                if (first)
                {
                    throw new JsonException("Unexpected comma");
                }
                Advance();
                peek = SkipWhitespace();
            }

            switch (peek)
            {
                case IByteReader.EndOfStream:
                    throw new InvalidOperationException("Unexpected end of stream");
                case (short)']':
                    Advance();
                    return;
                default:
                    // Assume we are at the start of a value and let Skip handle it
                    Skip();
                    break;
            }
            first = false;
        }
    }

    /// <summary>
    /// Scan a JSON number. If <paramref name="skipOnly"/> is true, this method will only skip the
    /// number and always returns a default span.
    ///
    /// See <a href="https://www.json.org/json-en.html" /> for the JSON number format.
    ///
    /// <paramref name="scratch"/> may be null if <paramref name="skipOnly"/> is true.
    /// </summary>
    private Utf8Span LexNumber(bool skipOnly, ScratchBuffer? scratch)
    {
        var b = PeekOrThrow();
        Debug.Assert(b is (byte)'-' or (>= (byte)'0' and <= (byte)'9'));

        // Optional leading minus sign
        switch (b)
        {
            case (byte)'-':
                AddOrSkip(b, skipOnly, scratch);
                Advance();
                b = PeekOrThrow();
                break;
        }

        short peek;

        // JSON does not allow leading zeros. If one is present, it cannot be followed by other
        // digits
        if (b == (byte)'0')
        {
            AddOrSkip(b, skipOnly, scratch);
            Advance();

            peek = Peek();
            if (peek is >= (short)'0' and <= (short)'9')
            {
                throw new JsonException("Leading zero not allowed");
            }
        }
        else
        {
            if (b is not >= (byte)'1' and <= (byte)'9')
            {
                throw new InvalidOperationException("expected 1-9");
            }

            AddOrSkip(b, skipOnly, scratch);
            Advance();

            // Lex integer part
            peek = LexDigits(skipOnly, scratch);
            if (peek == IByteReader.EndOfStream)
            {
                goto ReturnSpan;
            }
        }

        switch (peek)
        {
            case IByteReader.EndOfStream:
                goto ReturnSpan;
            case (short)'.':
                // Fractional part
                AddOrSkip((byte)'.', skipOnly, scratch);
                Advance();

                b = PeekOrThrow();
                if (b is not >= (byte)'0' and <= (byte)'9')
                {
                    throw new InvalidOperationException("expected 0-9");
                }

                peek = LexDigits(skipOnly, scratch);
                if (peek == IByteReader.EndOfStream)
                {
                    goto ReturnSpan;
                }
                b = (byte)peek;
                break;
            default:
                b = (byte)peek;
                break;
        }

        if (b is (byte)'e' or (byte)'E')
        {
            // Exponent part
            AddOrSkip(b, skipOnly, scratch);
            Advance();

            b = PeekOrThrow();
            if (b is (byte)'-' or (byte)'+')
            {
                AddOrSkip(b, skipOnly, scratch);
                Advance();
                b = PeekOrThrow();
            }
            if (b is not >= (byte)'0' and <= (byte)'9')
            {
                throw new InvalidOperationException("expected 0-9");
            }
            LexDigits(skipOnly, scratch);
        }

    ReturnSpan:
        if (skipOnly)
        {
            return default;
        }
        return scratch!.Span;
    }

    /// <summary>
    /// Lex a sequence of digits 0-9. Returns next character after the digits.
    ///
    /// <paramref name="scratch"/> may be null if <paramref name="skipOnly"/> is true.
    /// </summary>
    private short LexDigits(bool skipOnly, ScratchBuffer? scratch)
    {
        while (true)
        {
            var peek = Peek();
            switch (peek)
            {
                case >= (short)'0' and <= (short)'9':
                    AddOrSkip((byte)peek, skipOnly, scratch);
                    Advance();
                    continue;
            }
            return peek;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddOrSkip(byte b, bool skipOnly, ScratchBuffer? scratch)
    {
        if (skipOnly)
        {
            return;
        }
        scratch!.Add(b);
    }

    private byte PeekOrThrow() => Peek() switch
    {
        IByteReader.EndOfStream => throw new InvalidOperationException("Unexpected end of stream"),
        var s => (byte)s
    };


    public Utf8Span LexUtf8Span(ScratchBuffer scratch)
    {
        return byteReader.LexUtf8Span(skipOnly: false, scratch);
    }

    /// <summary>
    /// Reads a byte without advancing the stream, or returns <see cref="IByteReader.EndOfStream"/> if the end
    /// of the stream has been reached.
    /// </summary>
    public short Peek() => byteReader.Peek();

    public short SkipWhitespace()
    {
        while (true)
        {
            var s = byteReader.Peek();
            switch (s)
            {
                case (short)' ':
                case (short)'\t':
                case (short)'\n':
                case (short)'\r':
                    byteReader.Advance();
                    break;
                default:
                    return s;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Advance(int count = 1) => byteReader.Advance(count);
}

internal static class Utf8JsonReader_OldExtensions
{
    public static void ReadOrThrow(ref this Utf8JsonReader_Old reader)
    {
        if (!reader.Read())
        {
            throw new DeserializeException("Unexpected end of stream");
        }
    }
}