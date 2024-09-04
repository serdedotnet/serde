
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Serde.IO;

internal interface IByteReader
{
    public const short EndOfStream = -1;

    /// <summary>
    /// Reads a byte without advancing the stream, or returns <see cref="EndOfStream"/> if the end
    /// of the stream has been reached.
    /// </summary>
    short Peek();

    /// <summary>
    /// Reads a byte and advances the stream, or returns <see cref="EndOfStream"/> if the end of the
    /// stream has been reached.
    /// </summary>
    short Next();

    /// <summary>
    /// Advances the stream by <paramref name="count"/> bytes. The caller should ensure there is
    /// enough remaining data in the stream.
    /// </summary>
    void Advance(int count = 1);

    bool StartsWith(ReadOnlySpan<byte> span);

    /// <summary>
    /// Assumes we are one byte past the quote character. Reads a JSON string from the input.
    /// Returns the string as a <see cref="Utf8Span"/>. If <paramref name="skipOnly"/> is true, the
    /// string is not copied to the scratch buffer, and the returned <see cref="Utf8Span"/> is always
    /// empty.
    /// <paramref name="scratch"/> may be null if <paramref name="skipOnly"/> is true.
    /// </summary>
    Utf8Span LexUtf8Span(bool skipOnly, ScratchBuffer? scratch);
}