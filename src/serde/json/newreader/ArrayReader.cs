
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using Serde.Json;
using static Serde.Json.ThrowHelpers;

namespace Serde.IO;

internal struct ArrayReader(byte[] bytes) : IByteReader
{
    private readonly byte[] _bytes = bytes;
    private int _pos = 0;

    public short Next()
    {
        var b = Peek();
        if (b != IByteReader.EndOfStream)
        {
            _pos++;
        }
        return b;
    }

    public void Advance(int count = 1)
    {
        _pos += count;
    }

    public short Peek()
    {
        if (_pos >= _bytes.Length)
        {
            return IByteReader.EndOfStream;
        }
        return _bytes[_pos];
    }

    public bool StartsWith(Utf8Span span)
    {
        if (span.Length > _bytes.Length - _pos)
        {
            return false;
        }
        return span.SequenceEqual(_bytes.AsSpan(_pos, span.Length));
    }

    public Utf8Span LexUtf8Span(bool skipOnly, ScratchBuffer? scratch)
    {
        var span = _bytes.AsSpan();
        int start = _pos;
        while (true)
        {
            SkipToEscape();
            if (_pos == span.Length)
            {
                throw new InvalidOperationException("Unexpected end of stream.");
            }
            var b = span[_pos];
            switch (b)
            {
                case (byte)'"':
                    {
                        if (skipOnly)
                        {
                            Advance();
                            return Utf8Span.Empty;
                        }

                        Debug.Assert(scratch is not null);
                        var curSpan = span[start.._pos];
                        Utf8Span strSpan;
                        if (scratch.Count == 0)
                        {
                            strSpan = curSpan;
                        }
                        else
                        {
                            scratch.AddRange(curSpan);
                            strSpan = scratch.Span;
                        }
                        Advance();
                        return strSpan;
                    }
                case (byte)'\\':
                    {
                        if (!skipOnly)
                        {
                            scratch!.AddRange(span[start.._pos]);
                        }
                        Advance();
                        LexEscape(skipOnly, scratch);
                        start = _pos;
                        break;
                    }
                default:
                    {
                        Advance();
                        throw new InvalidOperationException("Invalid control character");
                    }
            }
        }
    }

    private void SkipToEscape()
    {
        var span = _bytes.AsSpan(_pos);
        var offset = 0;
        while (offset < span.Length && !IsEscape(span[offset], includingControlChars: true))
        {
            offset++;
        }
        _pos += offset;
    }

    /// <summary>
    /// Assumes we are one byte past the backslash character. Reads a JSON string escape sequence.
    ///
    /// If <paramref name="skipOnly"/> is true, the escape sequence is not copied to the scratch buffer.
    /// <paramref name="scratch"/> may be null if <paramref name="skipOnly"/> is true.
    /// </summary>
    private void LexEscape(bool skipOnly, ScratchBuffer? scratch)
    {
        var s = Next();
        ThrowIfEos(s);
        switch ((byte)s)
        {
            case (byte)'"': AddOrSkip('"', skipOnly, scratch); break;
            case (byte)'\\': AddOrSkip('\\', skipOnly, scratch); break;
            case (byte)'/': AddOrSkip('/', skipOnly, scratch); break;
            case (byte)'b': AddOrSkip('\b', skipOnly, scratch); break;
            case (byte)'f': AddOrSkip('\f', skipOnly, scratch); break;
            case (byte)'n': AddOrSkip('\n', skipOnly, scratch); break;
            case (byte)'r': AddOrSkip('\r', skipOnly, scratch); break;
            case (byte)'t': AddOrSkip('\t', skipOnly, scratch); break;
            case (byte)'u':
            {
                if (skipOnly)
                {
                    Advance(4);
                    break;
                }
                Debug.Assert(scratch is not null);
                var reqLen = scratch.Count + 5;
                scratch.EnsureCapacity(reqLen);
                var dest = scratch.BufferSpan[scratch.Count..];
                int written = 0;
                JsonReaderHelper.DecodeUnicodeEscape(_bytes, dest, ref _pos, ref written);
                scratch.Count += written;
                break;
            }
            default:
            {
                throw new InvalidOperationException($"Invalid escape character: {s}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddOrSkip(char c, bool skipOnly, ScratchBuffer? scratch)
        {
            if (!skipOnly)
            {
                scratch!.Add((byte)c);
            }
        }
    }

    private static bool IsEscape(byte b, bool includingControlChars)
    {
        return b == (byte)'"' ||
            b == (byte)'\\' ||
            (includingControlChars && (b < 0x20));
    }
}
