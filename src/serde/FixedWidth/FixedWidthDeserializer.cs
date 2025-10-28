// Contains implementations of data interfaces for core types

using System;
using System.Buffers;
using System.IO;
using System.Text;
using Serde.FixedWidth.Reader;
using Serde.IO;

namespace Serde.FixedWidth
{
    internal sealed class FixedWidthDeserializer
    {
        public static FixedWidthDeserializer<FixedWidthReader> FromString(string s)
            => FromUtf8_Unsafe(Encoding.UTF8.GetBytes(s));

        internal static FixedWidthDeserializer<FixedWidthReader> FromUtf8_Unsafe(byte[] utf8Bytes)
        {
            var reader = new FixedWidthReader();
            return new FixedWidthDeserializer<FixedWidthReader>(reader);
        }
    }

    /// <summary>
    /// Defines a type which handles deserializing a fixed-width file.
    /// </summary>
    internal sealed class FixedWidthDeserializer<TReader>(TReader byteReader) : IDeserializer
        where TReader : IByteReader
    {
        private readonly ScratchBuffer _scratch = new();

        /// <inheritdoc/>
        public string ReadString() => Encoding.UTF8.GetString(ReadUtf8Span());

        private Utf8Span ReadUtf8Span()
        {
            var peek = byteReader.Peek();
            if (peek == IByteReader.EndOfStream)
            {
                throw new EndOfStreamException();
            }

            byteReader.Advance();
            _scratch.Clear();
            return byteReader.LexUtf8Span(false, _scratch);
        }

        public void EoF()
        {
            if (byteReader.Peek() != IByteReader.EndOfStream)
            {
                throw new InvalidOperationException("Expected end of stream.");
            }
        }

        /// <inheritdoc/>
        public bool ReadBool() => throw new NotImplementedException();

        /// <inheritdoc/>
        public void ReadBytes(IBufferWriter<byte> writer) => throw new NotImplementedException();

        /// <inheritdoc/>
        public char ReadChar() => throw new NotImplementedException();

        /// <inheritdoc/>
        public DateTime ReadDateTime() => throw new NotImplementedException();

        /// <inheritdoc/>
        public decimal ReadDecimal() => throw new NotImplementedException();

        /// <inheritdoc/>
        public float ReadF32() => throw new NotImplementedException();

        /// <inheritdoc/>
        public double ReadF64() => throw new NotImplementedException();

        /// <inheritdoc/>
        public short ReadI16() => throw new NotImplementedException();

        /// <inheritdoc/>
        public int ReadI32() => throw new NotImplementedException();

        /// <inheritdoc/>
        public long ReadI64() => throw new NotImplementedException();

        /// <inheritdoc/>
        public sbyte ReadI8() => throw new NotImplementedException();

        /// <inheritdoc/>
        public T? ReadNullableRef<T>(IDeserialize<T> deserialize)
            where T : class => throw new NotImplementedException();

        /// <inheritdoc/>
        public ITypeDeserializer ReadType(ISerdeInfo typeInfo) => throw new NotImplementedException();

        /// <inheritdoc/>
        public ushort ReadU16() => throw new NotImplementedException();

        /// <inheritdoc/>
        public uint ReadU32() => throw new NotImplementedException();

        /// <inheritdoc/>
        public ulong ReadU64() => throw new NotImplementedException();

        /// <inheritdoc/>
        public byte ReadU8() => throw new NotImplementedException();

        /// <inheritdoc/>
        public void Dispose() => throw new NotImplementedException();
    }
}
