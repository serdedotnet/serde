using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Serde.Json;

partial class JsonSerializer
{
    private readonly Utf8JsonWriter _writer;
    private readonly KeySerializer _keySerializer;

    internal JsonSerializer(Utf8JsonWriter writer)
    {
        _writer = writer;
        _keySerializer = new KeySerializer(this);
    }
}

// Implementations of ISerializer
#if NET11_0_OR_GREATER
partial class JsonSerializer : ISerializer
{
    async Task ISerializer.WriteBool(bool b) { _writer.WriteBooleanValue(b); }
    async Task ISerializer.WriteChar(char c) { _writer.WriteStringValue(c.ToString()); }
    async Task ISerializer.WriteU8(byte b) { _writer.WriteNumberValue(b); }
    async Task ISerializer.WriteU16(ushort u16) { _writer.WriteNumberValue(u16); }
    async Task ISerializer.WriteU32(uint u32) { _writer.WriteNumberValue(u32); }
    async Task ISerializer.WriteU64(ulong u64) { _writer.WriteNumberValue(u64); }
    async Task ISerializer.WriteI8(sbyte b) { _writer.WriteNumberValue(b); }
    async Task ISerializer.WriteI16(short i16) { _writer.WriteNumberValue(i16); }
    async Task ISerializer.WriteI32(int i32) { _writer.WriteNumberValue(i32); }
    async Task ISerializer.WriteI64(long i64) { _writer.WriteNumberValue(i64); }
    async Task ISerializer.WriteF16(Half h) { _writer.WriteNumberValue((float)h); }
    async Task ISerializer.WriteF32(float f) { _writer.WriteNumberValue(f); }
    async Task ISerializer.WriteF64(double d) { _writer.WriteNumberValue(d); }
    async Task ISerializer.WriteDecimal(decimal d) { _writer.WriteNumberValue(d); }
    async Task ISerializer.WriteString(string s) { _writer.WriteStringValue(s); }
    async Task ISerializer.WriteNull() { _writer.WriteNullValue(); }
    async Task ISerializer.WriteDateTimeOffset(DateTimeOffset dt) { _writer.WriteStringValue(dt); }
    async Task ISerializer.WriteBytes(ReadOnlyMemory<byte> bytes) { _writer.WriteBase64StringValue(bytes.Span); }

    async Task ISerializer.WriteDateTime(DateTime dt)
    {
        if (dt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be in UTC");
        _writer.WriteStringValue(dt);
    }

    async Task ISerializer.WriteU128(UInt128 u128)
    {
        if (u128 <= ulong.MaxValue) _writer.WriteNumberValue((ulong)u128);
        else _writer.WriteRawValue(u128.ToString());
    }

    async Task ISerializer.WriteI128(Int128 i128)
    {
        if (i128 >= long.MinValue && i128 <= long.MaxValue) _writer.WriteNumberValue((long)i128);
        else _writer.WriteRawValue(i128.ToString());
    }

    async Task ISerializer.WriteEnum(ISerdeInfo info, int ordinal)
    {
        _writer.WriteStringValue(info.GetFieldName(ordinal));
    }

    async Task<ITypeSerializer> ISerializer.WriteCollection(ISerdeInfo info, int? size)
    {
        ITypeSerializer result = info.Kind switch
        {
            InfoKind.Dictionary => new DictImpl(this),
            InfoKind.List or InfoKind.Tuple => new EnumerableImpl(this),
            _ => throw new ArgumentException($"TypeKind is {info.Kind}, expected Enumerable or Dictionary"),
        };
        if (info.Kind == InfoKind.Dictionary) _writer.WriteStartObject(); else _writer.WriteStartArray();
        return result;
    }

    async Task<ITypeSerializer> ISerializer.WriteType(ISerdeInfo typeInfo, int fieldCount)
    {
        if (typeInfo.Kind is not (InfoKind.Union or InfoKind.CustomType))
        {
            throw new ArgumentException("Invalid type kind for WriteType: " + typeInfo.Kind);
        }
        _writer.WriteStartObject();
        return this;
    }

}
#else
partial class JsonSerializer : ISerializer
{
    void ISerializer.WriteBool(bool b) => _writer.WriteBooleanValue(b);
    void ISerializer.WriteChar(char c) => _writer.WriteStringValue(c.ToString());
    void ISerializer.WriteU8(byte b) => _writer.WriteNumberValue(b);
    void ISerializer.WriteU16(ushort u16) => _writer.WriteNumberValue(u16);
    void ISerializer.WriteU32(uint u32) => _writer.WriteNumberValue(u32);
    void ISerializer.WriteU64(ulong u64) => _writer.WriteNumberValue(u64);

    void ISerializer.WriteU128(UInt128 u128)
    {
        if (u128 <= ulong.MaxValue)
        {
            _writer.WriteNumberValue((ulong)u128);
            return;
        }
        Span<byte> buffer = stackalloc byte[39]; // max length of UInt128 in decimal is 39 digits
        if (!u128.TryFormat(buffer, out int written))
        {
            throw new InvalidOperationException("Failed to format UInt128: " + u128.ToString());
        }
        _writer.WriteRawValue(buffer.Slice(0, written));
    }

    void ISerializer.WriteI8(sbyte b) => _writer.WriteNumberValue(b);
    void ISerializer.WriteI16(short i16) => _writer.WriteNumberValue(i16);
    void ISerializer.WriteI32(int i32) => _writer.WriteNumberValue(i32);
    void ISerializer.WriteI64(long i64) => _writer.WriteNumberValue(i64);

    void ISerializer.WriteI128(Int128 i128)
    {
        if (i128 >= long.MinValue && i128 <= long.MaxValue)
        {
            _writer.WriteNumberValue((long)i128);
            return;
        }
        Span<byte> buffer = stackalloc byte[40]; // max length of Int128 in decimal is 39 digits + optional '-'
        if (!i128.TryFormat(buffer, out int written))
        {
            throw new InvalidOperationException("Failed to format Int128: " + i128.ToString());
        }
        _writer.WriteRawValue(buffer.Slice(0, written));
    }

    void ISerializer.WriteF16(Half h) => _writer.WriteNumberValue((float)h);
    void ISerializer.WriteF32(float f) => _writer.WriteNumberValue(f);
    void ISerializer.WriteF64(double d) => _writer.WriteNumberValue(d);
    void ISerializer.WriteDecimal(decimal d) => _writer.WriteNumberValue(d);
    void ISerializer.WriteString(string s) => _writer.WriteStringValue(s);
    void ISerializer.WriteNull() => _writer.WriteNullValue();

    void ISerializer.WriteDateTime(DateTime dt)
    {
        if (dt.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC");
        }
        _writer.WriteStringValue(dt);
    }

    void ISerializer.WriteDateTimeOffset(DateTimeOffset dt)
    {
        _writer.WriteStringValue(dt);
    }

    void ISerializer.WriteDateOnly(DateOnly d)
    {
        _writer.WriteStringValue(d.ToString("yyyy-MM-dd"));
    }

    void ISerializer.WriteTimeOnly(TimeOnly t)
    {
        _writer.WriteStringValue(t.ToString("HH:mm:ss"));
    }

    void ISerializer.WriteBytes(ReadOnlyMemory<byte> bytes) =>
        _writer.WriteBase64StringValue(bytes.Span);

    void ISerializer.WriteEnum(ISerdeInfo info, int ordinal)
    {
        var name = info.GetFieldName(ordinal);
        _writer.WriteStringValue(name);
    }

    ITypeSerializer ISerializer.WriteCollection(ISerdeInfo info, int? size)
    {
        switch (info.Kind)
        {
            case InfoKind.Dictionary:
                _writer.WriteStartObject();
                return new DictImpl(this);
            case InfoKind.List:
                _writer.WriteStartArray();
                return new EnumerableImpl(this);
            case InfoKind.Tuple:
                _writer.WriteStartArray();
                return new EnumerableImpl(this);
            default:
                throw new ArgumentException(
                    $"TypeKind is {info.Kind}, expected Enumerable or Dictionary"
                );
        }
    }

    ITypeSerializer ISerializer.WriteType(ISerdeInfo typeInfo) => WriteType(typeInfo);

    ITypeSerializer ISerializer.WriteType(ISerdeInfo typeInfo, int fieldCount) =>
        WriteType(typeInfo);

    private ITypeSerializer WriteType(ISerdeInfo typeInfo)
    {
        switch (typeInfo.Kind)
        {
            case InfoKind.Union:
            case InfoKind.CustomType:
                _writer.WriteStartObject();
                return this;
            case InfoKind.Enum:
            default:
                throw new ArgumentException("Invalid type kind for WriteType: " + typeInfo.Kind);
        }
    }
}
#endif

/// <summary>
/// Implements ITypeSerializer for custom types.
/// </summary>
partial class JsonSerializer : ITypeSerializer
{
#if NET11_0_OR_GREATER
    async Task<ISerializer> ITypeSerializer.WriteFieldStart(ISerdeInfo info, int index)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        return this;
    }

    async Task ITypeSerializer.WriteFieldEnd(ISerdeInfo info, int index, ISerializer serializer) { }

    async Task ITypeSerializer.WriteValue<T>(
        ISerdeInfo info, int index, T value, ISerialize<T> serialize
    )
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        await serialize.Serialize(value, this);
    }

    async Task ITypeSerializer.End(ISerdeInfo info)
    {
        _writer.WriteEndObject();
    }

    async Task ITypeSerializer.WriteBool(ISerdeInfo info, int index, bool value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteBooleanValue(value);
    }

    async Task ITypeSerializer.WriteChar(ISerdeInfo info, int index, char value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteStringValue(value.ToString());
    }

    async Task ITypeSerializer.WriteU8(ISerdeInfo info, int index, byte value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteU16(ISerdeInfo info, int index, ushort value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteU32(ISerdeInfo info, int index, uint value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteU64(ISerdeInfo info, int index, ulong value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteU128(ISerdeInfo info, int index, UInt128 value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        if (value <= ulong.MaxValue) _writer.WriteNumberValue((ulong)value);
        else _writer.WriteRawValue(value.ToString());
    }

    async Task ITypeSerializer.WriteI8(ISerdeInfo info, int index, sbyte value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteI16(ISerdeInfo info, int index, short value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteI32(ISerdeInfo info, int index, int value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteI64(ISerdeInfo info, int index, long value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteI128(ISerdeInfo info, int index, Int128 value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        if (value >= long.MinValue && value <= long.MaxValue) _writer.WriteNumberValue((long)value);
        else _writer.WriteRawValue(value.ToString());
    }

    async Task ITypeSerializer.WriteF16(ISerdeInfo info, int index, Half value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue((float)value);
    }

    async Task ITypeSerializer.WriteF32(ISerdeInfo info, int index, float value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteF64(ISerdeInfo info, int index, double value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteDecimal(ISerdeInfo info, int index, decimal value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNumberValue(value);
    }

    async Task ITypeSerializer.WriteString(ISerdeInfo info, int index, string value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteStringValue(value);
    }

    async Task ITypeSerializer.WriteNull(ISerdeInfo info, int index)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteNullValue();
    }

    async Task ITypeSerializer.WriteDateTime(ISerdeInfo info, int index, DateTime value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        if (value.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be in UTC");
        _writer.WriteStringValue(value);
    }

    async Task ITypeSerializer.WriteDateTimeOffset(ISerdeInfo info, int index, DateTimeOffset value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteStringValue(value);
    }

    async Task ITypeSerializer.WriteDateOnly(ISerdeInfo info, int index, DateOnly value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }

    async Task ITypeSerializer.WriteTimeOnly(ISerdeInfo info, int index, TimeOnly value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteStringValue(value.ToString("HH:mm:ss"));
    }

    async Task ITypeSerializer.WriteBytes(ISerdeInfo info, int index, ReadOnlyMemory<byte> value)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteBase64StringValue(value.Span);
    }

    async Task ITypeSerializer.WriteEnum(ISerdeInfo info, int index, ISerdeInfo fieldInfo, int ordinal)
    {
        _writer.WritePropertyName(info.GetFieldName(index));
        _writer.WriteStringValue(fieldInfo.GetFieldName(ordinal));
    }
#else
    ISerializer ITypeSerializer.WriteFieldStart(ISerdeInfo typeInfo, int fieldIndex)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(fieldIndex));
        return this;
    }

    void ITypeSerializer.WriteFieldEnd(ISerdeInfo typeInfo, int fieldIndex, ISerializer serializer)
    {
        // No-op for JSON
    }

    void ITypeSerializer.WriteValue<T>(
        ISerdeInfo typeInfo,
        int fieldIndex,
        T value,
        ISerialize<T> serialize
    )
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(fieldIndex));
        serialize.Serialize(value, this);
    }

    void ITypeSerializer.End(ISerdeInfo typeInfo)
    {
        _writer.WriteEndObject();
    }

    void ITypeSerializer.WriteBool(ISerdeInfo typeInfo, int index, bool b)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteBooleanValue(b);
    }

    void ITypeSerializer.WriteChar(ISerdeInfo typeInfo, int index, char c)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteStringValue(c.ToString());
    }

    void ITypeSerializer.WriteU8(ISerdeInfo typeInfo, int index, byte b)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(b);
    }

    void ITypeSerializer.WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(u16);
    }

    void ITypeSerializer.WriteU32(ISerdeInfo typeInfo, int index, uint u32)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(u32);
    }

    void ITypeSerializer.WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(u64);
    }

    void ITypeSerializer.WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        if (u128 <= ulong.MaxValue) _writer.WriteNumberValue((ulong)u128);
        else _writer.WriteRawValue(u128.ToString());
    }

    void ITypeSerializer.WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(b);
    }

    void ITypeSerializer.WriteI16(ISerdeInfo typeInfo, int index, short i16)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(i16);
    }

    void ITypeSerializer.WriteI32(ISerdeInfo typeInfo, int index, int i32)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(i32);
    }

    void ITypeSerializer.WriteI64(ISerdeInfo typeInfo, int index, long i64)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(i64);
    }

    void ITypeSerializer.WriteI128(ISerdeInfo typeInfo, int index, Int128 i128)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        if (i128 >= long.MinValue && i128 <= long.MaxValue) _writer.WriteNumberValue((long)i128);
        else _writer.WriteRawValue(i128.ToString());
    }

    void ITypeSerializer.WriteF16(ISerdeInfo typeInfo, int index, Half h)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue((float)h);
    }

    void ITypeSerializer.WriteF32(ISerdeInfo typeInfo, int index, float f)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(f);
    }

    void ITypeSerializer.WriteF64(ISerdeInfo typeInfo, int index, double d)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(d);
    }

    void ITypeSerializer.WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNumberValue(d);
    }

    void ITypeSerializer.WriteString(ISerdeInfo typeInfo, int index, string s)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteStringValue(s);
    }

    void ITypeSerializer.WriteNull(ISerdeInfo typeInfo, int index)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteNullValue();
    }

    void ITypeSerializer.WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        if (dt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime must be in UTC");
        _writer.WriteStringValue(dt);
    }

    void ITypeSerializer.WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteStringValue(dt);
    }

    void ITypeSerializer.WriteDateOnly(ISerdeInfo typeInfo, int index, DateOnly d)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteStringValue(d.ToString("yyyy-MM-dd"));
    }

    void ITypeSerializer.WriteTimeOnly(ISerdeInfo typeInfo, int index, TimeOnly t)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteStringValue(t.ToString("HH:mm:ss"));
    }

    void ITypeSerializer.WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteBase64StringValue(bytes.Span);
    }

    void ITypeSerializer.WriteEnum(
        ISerdeInfo typeInfo,
        int index,
        ISerdeInfo fieldInfo,
        int ordinal
    )
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        _writer.WriteStringValue(fieldInfo.GetFieldName(ordinal));
    }
#endif
}
