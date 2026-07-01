using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

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
partial class JsonSerializer : ISerializer
{
    public void WriteBool(bool b) => _writer.WriteBooleanValue(b);

    public void WriteChar(char c) => WriteString(c.ToString());

    public void WriteU8(byte b) => _writer.WriteNumberValue(b);

    public void WriteU16(ushort u16) => _writer.WriteNumberValue(u16);

    public void WriteU32(uint u32) => _writer.WriteNumberValue(u32);

    public void WriteU64(ulong u64) => _writer.WriteNumberValue(u64);

    public void WriteU128(UInt128 u128)
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

    public void WriteI8(sbyte b) => _writer.WriteNumberValue(b);

    public void WriteI16(short i16) => _writer.WriteNumberValue(i16);

    public void WriteI32(int i32) => _writer.WriteNumberValue(i32);

    public void WriteI64(long i64) => _writer.WriteNumberValue(i64);

    public void WriteI128(Int128 i128)
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

    public void WriteF16(Half h) => _writer.WriteNumberValue((float)h);

    public void WriteF32(float f) => _writer.WriteNumberValue(f);

    public void WriteF64(double d) => _writer.WriteNumberValue(d);

    public void WriteDecimal(decimal d) => _writer.WriteNumberValue(d);

    public void WriteString(string s) => _writer.WriteStringValue(s);

    public void WriteNull() => _writer.WriteNullValue();

    public void WriteDateTime(DateTime dt)
    {
        if (dt.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime must be in UTC");
        }
        _writer.WriteStringValue(dt);
    }

    public void WriteDateTimeOffset(DateTimeOffset dt)
    {
        _writer.WriteStringValue(dt);
    }

    public void WriteDateOnly(DateOnly d)
    {
        _writer.WriteStringValue(d.ToString("yyyy-MM-dd"));
    }

    public void WriteTimeOnly(TimeOnly t)
    {
        _writer.WriteStringValue(t.ToString("HH:mm:ss"));
    }

    public void WriteBytes(ReadOnlyMemory<byte> bytes) =>
        _writer.WriteBase64StringValue(bytes.Span);

    public void WriteEnum(ISerdeInfo info, int ordinal)
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

    ITypeSerializer ISerializer.WriteType(ISerdeInfo typeInfo)
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

/// <summary>
/// Implements ITypeSerializer for custom types.
/// </summary>
partial class JsonSerializer : ITypeSerializer
{
    public ISerializer WriteFieldStart(ISerdeInfo typeInfo, int fieldIndex)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(fieldIndex));
        return this;
    }

    public void WriteFieldEnd(ISerdeInfo typeInfo, int fieldIndex, ISerializer serializer)
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
        WriteBool(b);
    }

    void ITypeSerializer.WriteChar(ISerdeInfo typeInfo, int index, char c)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteChar(c);
    }

    void ITypeSerializer.WriteU8(ISerdeInfo typeInfo, int index, byte b)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteU8(b);
    }

    void ITypeSerializer.WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteU16(u16);
    }

    void ITypeSerializer.WriteU32(ISerdeInfo typeInfo, int index, uint u32)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteU32(u32);
    }

    void ITypeSerializer.WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteU64(u64);
    }

    void ITypeSerializer.WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteU128(u128);
    }

    void ITypeSerializer.WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteI8(b);
    }

    void ITypeSerializer.WriteI16(ISerdeInfo typeInfo, int index, short i16)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteI16(i16);
    }

    void ITypeSerializer.WriteI32(ISerdeInfo typeInfo, int index, int i32)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteI32(i32);
    }

    void ITypeSerializer.WriteI64(ISerdeInfo typeInfo, int index, long i64)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteI64(i64);
    }

    void ITypeSerializer.WriteI128(ISerdeInfo typeInfo, int index, Int128 i128)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteI128(i128);
    }

    void ITypeSerializer.WriteF16(ISerdeInfo typeInfo, int index, Half h)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteF16(h);
    }

    void ITypeSerializer.WriteF32(ISerdeInfo typeInfo, int index, float f)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteF32(f);
    }

    void ITypeSerializer.WriteF64(ISerdeInfo typeInfo, int index, double d)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteF64(d);
    }

    void ITypeSerializer.WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteDecimal(d);
    }

    void ITypeSerializer.WriteString(ISerdeInfo typeInfo, int index, string s)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteString(s);
    }

    void ITypeSerializer.WriteNull(ISerdeInfo typeInfo, int index)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteNull();
    }

    void ITypeSerializer.WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteDateTime(dt);
    }

    void ITypeSerializer.WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteDateTimeOffset(dt);
    }

    void ITypeSerializer.WriteDateOnly(ISerdeInfo typeInfo, int index, DateOnly d)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteDateOnly(d);
    }

    void ITypeSerializer.WriteTimeOnly(ISerdeInfo typeInfo, int index, TimeOnly t)
    {
        _writer.WritePropertyName(typeInfo.GetFieldName(index));
        WriteTimeOnly(t);
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
        WriteEnum(fieldInfo, ordinal);
    }
}
