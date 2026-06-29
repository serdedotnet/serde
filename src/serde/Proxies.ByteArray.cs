// Contains implementations of data interfaces for core types

using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;

namespace Serde;

public sealed class ByteArrayProxy : ISerdePrimitive<ByteArrayProxy, byte[]>
{
    private sealed class ArrayWriter : IBufferWriter<byte>
    {
        public byte[]? _buffer;
        public int _written = 0;

        public void Advance(int count)
        {
            if (_buffer is null)
            {
                throw new InvalidOperationException("Buffer is null");
            }
            _written = count;
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (sizeHint == 0)
            {
                sizeHint = 1;
            }
            if (_buffer == null || _buffer.Length < sizeHint)
            {
                _buffer = new byte[sizeHint];
            }
            return _buffer;
        }

        public Span<byte> GetSpan(int sizeHint = 0) => GetMemory(sizeHint).Span;

        public void Clear()
        {
            _written = 0;
            _buffer = null;
        }

        public byte[] GetArray()
        {
            var buffer = _buffer;
            if (buffer is null)
            {
                Debug.Assert(_written == 0);
                return Array.Empty<byte>();
            }
            if (_written != buffer.Length)
            {
                var newBuffer = new byte[_written];
                Array.Copy(buffer, newBuffer, _written);
                return newBuffer;
            }
            return buffer;
        }
    }

    private ArrayWriter? _bufferWriter = new();

    private (ArrayWriter, bool Owned) BorrowBufferWriter()
    {
        var bufferWriter = Interlocked.Exchange(ref _bufferWriter, null);
        if (bufferWriter is null)
        {
            return (new ArrayWriter(), false);
        }
        return (bufferWriter, true);
    }

    private void ReturnBufferWriter(ArrayWriter bufferWriter)
    {
        bufferWriter.Clear();
        if (Interlocked.Exchange(ref _bufferWriter, bufferWriter) is not null)
        {
            throw new InvalidOperationException("Buffer writer released twice");
        }
    }

    public static ByteArrayProxy Instance { get; } = new();

    private ByteArrayProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("byte[]", PrimitiveKind.Bytes);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<byte[]>.Serialize(byte[] value, ISerializer serializer) =>
        serializer.WriteBytes(value);

    byte[] IDeserialize<byte[]>.Deserialize(IDeserializer deserializer)
    {
        // Take ownership of the buffer writer
        var (bufferWriter, owned) = BorrowBufferWriter();
        try
        {
            deserializer.ReadBytes(bufferWriter);
            return bufferWriter.GetArray();
        }
        finally
        {
            if (owned)
            {
                ReturnBufferWriter(bufferWriter);
            }
        }
    }

    void ISerialize<byte[]>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        byte[] value
    )
    {
        serializer.WriteBytes(info, index, value);
    }

    byte[] IDeserialize<byte[]>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    )
    {
        // Take ownership of the buffer writer
        var (bufferWriter, owned) = BorrowBufferWriter();
        try
        {
            deserializer.ReadBytes(info, index, bufferWriter);
            return bufferWriter.GetArray();
        }
        finally
        {
            if (owned)
            {
                ReturnBufferWriter(bufferWriter);
            }
        }
    }
}
