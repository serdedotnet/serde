using System;
using System.Diagnostics;
using System.IO;
using Serde.IO;

namespace Serde.FixedWidth.Reader
{
    internal struct FixedWidthReader(byte[] bytes) : IByteReader
    {
        private readonly byte[] _bytes = bytes;
        private int _pos = 0;

        /// <inheritdoc/>
        public short Next()
        {
            var b = Peek();
            if (b != IByteReader.EndOfStream)
            {
                _pos++;
            }

            return b;
        }

        /// <inheritdoc/>
        public void Advance(int count = 1)
        {
            _pos += count;
        }

        /// <inheritdoc/>
        public readonly short Peek()
        {
            if (_pos >= _bytes.Length)
            {
                return IByteReader.EndOfStream;
            }
            return _bytes[_pos];
        }

        /// <inheritdoc/>
        public readonly bool StartsWith(Utf8Span span)
        {
            if (span.Length > _bytes.Length - _pos)
            {
                return false;
            }

            return span.SequenceEqual(_bytes.AsSpan(_pos, span.Length));
        }

        /// <inheritdoc/>
        public Utf8Span LexUtf8Span(bool skipOnly, ScratchBuffer? scratch)
        {
            var span = _bytes.AsSpan();
            int start = _pos;

            SkipToEndOfLine();
            if (_pos >= span.Length)
            {
                throw new EndOfStreamException();
            }

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

        private void SkipToEndOfLine()
        {
            var span = _bytes.AsSpan(_pos);
            var offset = 0;
            while (offset < span.Length && !IsEndOfLine(span[offset]))
            {
                offset++;
            }

            _pos += offset;
        }

        private static bool IsEndOfLine(byte b)
        {
            return b == (byte)'\r' || b == (byte)'\n';
        }
    }
}
