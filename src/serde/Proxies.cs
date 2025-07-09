// Contains implementations of data interfaces for core types

using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Serde;

internal interface ISerdePrimitive<TSelf, T>
    : ISerde<T>, ISerdeProvider<TSelf, TSelf, T>, ITypeSerialize<T>, ITypeDeserialize<T>
    where TSelf : ISerdePrimitive<TSelf, T>
{
    /// <summary>
    /// Abstract static to force all primitives to provide a convenient static accessor.
    /// </summary>
    public new abstract static ISerdeInfo SerdeInfo { get; }
}

public sealed class BoolProxy : ISerdePrimitive<BoolProxy, bool>
{
    public static BoolProxy Instance { get; } = new();
    private BoolProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.Bool);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "bool";
    void ISerialize<bool>.Serialize(bool value, ISerializer serializer)
        => serializer.WriteBool(value);
    Task<bool> IDeserialize<bool>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadBool();
    void ITypeSerialize<bool>.Serialize(bool value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteBool(info, index, value);
    Task<bool> ITypeDeserialize<bool>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadBool(info, index);
}

public sealed class CharProxy : ISerdePrimitive<CharProxy, char>
{
    public static CharProxy Instance { get; } = new();
    private CharProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.Char);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "char";
    void ISerialize<char>.Serialize(char value, ISerializer serializer)
        => serializer.WriteChar(value);
    Task<char> IDeserialize<char>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadChar();
    void ITypeSerialize<char>.Serialize(char value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteChar(info, index, value);
    Task<char> ITypeDeserialize<char>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadChar(info, index);
}

public sealed class U8Proxy : ISerdePrimitive<U8Proxy, byte>
{
    public static U8Proxy Instance { get; } = new();
    private U8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U8);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "byte";
    void ISerialize<byte>.Serialize(byte value, ISerializer serializer)
        => serializer.WriteU8(value);
    Task<byte> IDeserialize<byte>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU8();
    void ITypeSerialize<byte>.Serialize(byte value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU8(info, index, value);
    Task<byte> ITypeDeserialize<byte>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU8(info, index);
}

public sealed class U16Proxy : ISerdePrimitive<U16Proxy, ushort>
{
    public static U16Proxy Instance { get; } = new();
    private U16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U16);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "ushort";
    void ISerialize<ushort>.Serialize(ushort value, ISerializer serializer)
        => serializer.WriteU16(value);
    Task<ushort> IDeserialize<ushort>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU16();
    Task<ushort> ITypeDeserialize<ushort>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU16(info, index);

    void ITypeSerialize<ushort>.Serialize(ushort value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU16(info, index, value);
}

public sealed class U32Proxy : ISerdePrimitive<U32Proxy, uint>
{
    public static U32Proxy Instance { get; } = new();
    private U32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U32);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "uint";
    void ISerialize<uint>.Serialize(uint value, ISerializer serializer)
        => serializer.WriteU32(value);

    Task<uint> IDeserialize<uint>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU32();
    Task<uint> ITypeDeserialize<uint>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU32(info, index);

    void ITypeSerialize<uint>.Serialize(uint value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU32(info, index, value);
}

public sealed class U64Proxy : ISerdePrimitive<U64Proxy, ulong>
{
    public static U64Proxy Instance { get; } = new();
    private U64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.U64);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "ulong";
    void ISerialize<ulong>.Serialize(ulong value, ISerializer serializer)
        => serializer.WriteU64(value);
    Task<ulong> IDeserialize<ulong>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadU64();

    void ITypeSerialize<ulong>.Serialize(ulong value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteU64(info, index, value);
    Task<ulong> ITypeDeserialize<ulong>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadU64(info, index);
}

public sealed class I8Proxy : ISerdePrimitive<I8Proxy, sbyte>
{
    public static I8Proxy Instance { get; } = new();
    private I8Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I8);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "sbyte";
    void ISerialize<sbyte>.Serialize(sbyte value, ISerializer serializer)
        => serializer.WriteI8(value);
    Task<sbyte> IDeserialize<sbyte>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI8();
    void ITypeSerialize<sbyte>.Serialize(sbyte value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI8(info, index, value);
    Task<sbyte> ITypeDeserialize<sbyte>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI8(info, index);
}

public sealed class I16Proxy : ISerdePrimitive<I16Proxy, short>
{
    public static I16Proxy Instance { get; } = new();
    private I16Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I16);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "short";
    void ISerialize<short>.Serialize(short value, ISerializer serializer)
        => serializer.WriteI16(value);
    Task<short> IDeserialize<short>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI16();
    void ITypeSerialize<short>.Serialize(short value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI16(info, index, value);
    Task<short> ITypeDeserialize<short>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI16(info, index);
}

public sealed class I32Proxy : ISerdePrimitive<I32Proxy, int>
{
    public static I32Proxy Instance { get; } = new();
    private I32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I32);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "int";
    void ISerialize<int>.Serialize(int value, ISerializer serializer)
        => serializer.WriteI32(value);
    void ITypeSerialize<int>.Serialize(int value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI32(info, index, value);
    Task<int> ITypeDeserialize<int>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI32(info, index);
    Task<int> IDeserialize<int>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI32();
}

public sealed class I64Proxy : ISerdePrimitive<I64Proxy, long>
{
    public static I64Proxy Instance { get; } = new();
    private I64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.I64);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "long";
    void ISerialize<long>.Serialize(long value, ISerializer serializer)
        => serializer.WriteI64(value);
    void ITypeSerialize<long>.Serialize(long value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteI64(info, index, value);
    Task<long> ITypeDeserialize<long>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadI64(info, index);
    Task<long> IDeserialize<long>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadI64();
}

public sealed class F32Proxy : ISerdePrimitive<F32Proxy, float>
{
    public static F32Proxy Instance { get; } = new();
    private F32Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("float", PrimitiveKind.F32);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    public void Serialize(float value, ISerializer serializer)
        => serializer.WriteF32(value);
    void ITypeSerialize<float>.Serialize(float value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteF32(info, index, value);
    Task<float> ITypeDeserialize<float>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadF32(info, index);
    Task<float> IDeserialize<float>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadF32();
}

public sealed class F64Proxy : ISerdePrimitive<F64Proxy, double>
{
    public static F64Proxy Instance { get; } = new();
    private F64Proxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("double", PrimitiveKind.F64);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<double>.Serialize(double value, ISerializer serializer)
        => serializer.WriteF64(value);
    void ITypeSerialize<double>.Serialize(double value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteF64(info, index, value);
    Task<double> ITypeDeserialize<double>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadF64(info, index);
    Task<double> IDeserialize<double>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadF64();
}

public sealed class DecimalProxy : ISerdePrimitive<DecimalProxy, decimal>
{
    public static DecimalProxy Instance { get; } = new();
    private DecimalProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("decimal", PrimitiveKind.Decimal);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<decimal>.Serialize(decimal value, ISerializer serializer)
        => serializer.WriteDecimal(value);
    void ITypeSerialize<decimal>.Serialize(decimal value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDecimal(info, index, value);
    Task<decimal> ITypeDeserialize<decimal>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDecimal(info, index);
    Task<decimal> IDeserialize<decimal>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDecimal();
}

public sealed class StringProxy : ISerdePrimitive<StringProxy, string>
{
    public static StringProxy Instance { get; } = new();
    private StringProxy() { }

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(s_typeName, PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    private const string s_typeName = "string";
    void ISerialize<string>.Serialize(string value, ISerializer serializer)
        => serializer.WriteString(value);
    void ITypeSerialize<string>.Serialize(string value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteString(info, index, value);
    Task<string> ITypeDeserialize<string>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadString(info, index);
    Task<string> IDeserialize<string>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadString();
}

public static class BoxProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<object?>, ITypeSerialize<T>
        where TProvider : ISerializeProvider<T>
    {
        private readonly ISerialize<T> _underlying = TProvider.Instance;

        public static readonly Ser<T, TProvider> Instance = new();

        private Ser() {}
        public ISerdeInfo SerdeInfo => _underlying.SerdeInfo;

        public void Serialize(object? value, ISerializer serializer)
        {
            _underlying.Serialize((T)value!, serializer);
        }
        public void Serialize(T value, ITypeSerializer serializer, ISerdeInfo info, int index)
        {
            serializer.WriteValue(info, index, value, this);
        }
    }

    public sealed class De<T, TProvider> : IDeserialize<object?>, ITypeDeserialize<T>
        where TProvider : IDeserializeProvider<T>
    {
        private IDeserialize<T> _underlying = TProvider.Instance;

        public static readonly De<T, TProvider> Instance = new();
        private De() {}

        public ISerdeInfo SerdeInfo => _underlying.SerdeInfo;
        public async Task<object?> Deserialize(IDeserializer deserializer)
            => await _underlying.Deserialize(deserializer).ConfigureAwait(false);
        public async Task<T> Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
            => await deserializer.ReadValue(info, index, this).ConfigureAwait(false) is T t ? t : default!;

        // Explicit interface implementations
        Task<object?> IDeserialize<object?>.Deserialize(IDeserializer deserializer)
            => Deserialize(deserializer);
        Task<T> ITypeDeserialize<T>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
            => Deserialize(deserializer, info, index);
    }
}


public static class NullableProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<T?>, ISerializeProvider<T?>
        where T : struct
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly ISerialize<T> proxy = TProvider.Instance;
        private Ser() {}
        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is {} notnull)
            {
                proxy.Serialize(notnull, serializer);
            }
            else
            {
                serializer.WriteNull();
            }
        }
    }

    public sealed class De<T, TProvider> : IDeserialize<T?>, IDeserializeProvider<T?>
        where T : struct
        where TProvider : IDeserializeProvider<T>
    {
        public static De<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);
        private De() {}

        public async Task<T?> Deserialize(IDeserializer deserializer)
        {
            var result = await deserializer.ReadNullableRef<object>(BoxProxy.De<T, TProvider>.Instance!).ConfigureAwait(false);
            return (T?)result;
        }
        Task<T?> IDeserialize<T?>.Deserialize(IDeserializer deserializer) => Deserialize(deserializer);
    }
}

public static class NullableRefProxy
{
    public sealed class Ser<T, TProvider> : ISerialize<T?>, ISerializeProvider<T?>
        where T : class?
        where TProvider : ISerializeProvider<T>
    {
        public static Ser<T, TProvider> Instance { get; } = new();
        static ISerialize<T?> ISerializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly ISerialize<T> _ser = TProvider.Instance;

        private Ser() {}

        void ISerialize<T?>.Serialize(T? value, ISerializer serializer)
        {
            if (value is null)
            {
                serializer.WriteNull();
            }
            else
            {
                _ser.Serialize(value, serializer);
            }
        }
    }

    public sealed class De<T, TProvider> : IDeserialize<T?>, IDeserializeProvider<T?>
        where T : class
        where TProvider : IDeserializeProvider<T>
    {
        public static De<T, TProvider> Instance { get; } = new();
        static IDeserialize<T?> IDeserializeProvider<T?>.Instance => Instance;
        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeNullable(TProvider.Instance.SerdeInfo);

        private readonly IDeserialize<T> _de = TProvider.Instance;

        private De() { }

        public Task<T?> Deserialize(IDeserializer deserializer)
        {
            return deserializer.ReadNullableRef(_de);
        }
        Task<T?> IDeserialize<T?>.Deserialize(IDeserializer deserializer) => Deserialize(deserializer);
    }
}

public sealed class GuidProxy : ISerdePrimitive<GuidProxy, Guid>
{
    public static GuidProxy Instance { get; } = new();
    private GuidProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.Guid", PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<Guid>.Serialize(Guid value, ISerializer serializer)
    {
        var bytes = value.ToString();
        serializer.WriteString(bytes);
    }

    void ITypeSerialize<Guid>.Serialize(Guid value, ITypeSerializer serializer, ISerdeInfo info, int index)
    {
        var bytes = value.ToString();
        serializer.WriteString(info, index, bytes);
    }

    async Task<Guid> ITypeDeserialize<Guid>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
    {
        var bytes = await deserializer.ReadString(info, index).ConfigureAwait(false);
        return Guid.Parse(bytes);
    }

    async Task<Guid> IDeserialize<Guid>.Deserialize(IDeserializer deserializer)
    {
        var str = await deserializer.ReadString().ConfigureAwait(false);
        return Guid.Parse(str);
    }
}

public sealed class DateTimeProxy : ISerdePrimitive<DateTimeProxy, DateTime>
{
    public static DateTimeProxy Instance { get; } = new();
    private DateTimeProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.DateTime", PrimitiveKind.DateTime);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<DateTime>.Serialize(DateTime value, ISerializer serializer)
        => serializer.WriteDateTime(value);
    void ITypeSerialize<DateTime>.Serialize(DateTime value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDateTime(info, index, value);
    Task<DateTime> ITypeDeserialize<DateTime>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDateTime(info, index);
    Task<DateTime> IDeserialize<DateTime>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDateTime();
}

public sealed class DateTimeOffsetProxy : ISerdePrimitive<DateTimeOffsetProxy, DateTimeOffset>
{
    public static DateTimeOffsetProxy Instance { get; } = new();
    private DateTimeOffsetProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.DateTimeOffset", PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<DateTimeOffset>.Serialize(DateTimeOffset value, ISerializer serializer)
        => serializer.WriteDateTimeOffset(value);
    void ITypeSerialize<DateTimeOffset>.Serialize(DateTimeOffset value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDateTimeOffset(info, index, value);

    async Task<DateTimeOffset> ITypeDeserialize<DateTimeOffset>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
    {
        var dt = await deserializer.ReadDateTime(info, index).ConfigureAwait(false);
        return new DateTimeOffset(dt);
    }
    async Task<DateTimeOffset> IDeserialize<DateTimeOffset>.Deserialize(IDeserializer deserializer)
    {
        var dt = await deserializer.ReadDateTime().ConfigureAwait(false);
        return new DateTimeOffset(dt);
    }
}


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

    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive("byte[]", PrimitiveKind.Bytes);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<byte[]>.Serialize(byte[] value, ISerializer serializer)
        => serializer.WriteBytes(value);
    void ITypeSerialize<byte[]>.Serialize(byte[] value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteBytes(info, index, value);

    // Remove all sync/duplicate implementations and make all deserialization async
    public async Task<byte[]> DeserializeAsync(IDeserializer deserializer)
    {
        var (bufferWriter, owned) = BorrowBufferWriter();
        try
        {
            await deserializer.ReadBytes(bufferWriter).ConfigureAwait(false);
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

    public async Task<byte[]> DeserializeAsync(ITypeDeserializer deserializer, ISerdeInfo info, int index)
    {
        var (bufferWriter, owned) = BorrowBufferWriter();
        try
        {
            await deserializer.ReadBytes(info, index, bufferWriter).ConfigureAwait(false);
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

    Task<byte[]> IDeserialize<byte[]>.Deserialize(IDeserializer deserializer)
        => DeserializeAsync(deserializer);

    Task<byte[]> ITypeDeserialize<byte[]>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => DeserializeAsync(deserializer, info, index);
}