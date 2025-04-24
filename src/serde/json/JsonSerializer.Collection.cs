
using System;

namespace Serde.Json;

partial class JsonSerializer
{
    private sealed class EnumerableImpl(JsonSerializer serializer) : ITypeSerializer
    {
        public void End(ISerdeInfo info)
        {
            serializer._writer.WriteEndArray();
        }

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b)
        {
            serializer.WriteBool(b);
        }

        public void WriteChar(ISerdeInfo typeInfo, int index, char c)
        {
            serializer.WriteChar(c);
        }

        public void WriteU8(ISerdeInfo typeInfo, int index, byte b)
        {
            serializer.WriteU8(b);
        }

        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) => serializer.WriteU16(u16);

        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32)
        {
            serializer.WriteU32(u32);
        }

        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
        {
            serializer.WriteU64(u64);
        }

        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
        {
            serializer.WriteI8(b);
        }

        public void WriteI16(ISerdeInfo typeInfo, int index, short i16)
        {
            serializer.WriteI16(i16);
        }

        public void WriteI32(ISerdeInfo typeInfo, int index, int i32)
        {
            serializer.WriteI32(i32);
        }

        public void WriteI64(ISerdeInfo typeInfo, int index, long i64)
        {
            serializer.WriteI64(i64);
        }

        public void WriteF32(ISerdeInfo typeInfo, int index, float f)
        {
            serializer.WriteF32(f);
        }

        public void WriteF64(ISerdeInfo typeInfo, int index, double d)
        {
            serializer.WriteF64(d);
        }

        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
        {
            serializer.WriteDecimal(d);
        }

        public void WriteString(ISerdeInfo typeInfo, int index, string s)
        {
            serializer.WriteString(s);
        }

        public void WriteNull(ISerdeInfo typeInfo, int index)
        {
            serializer.WriteNull();
        }

        public void WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt)
        {
            serializer.WriteDateTime(dt);
        }
        public void WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt)
        {
            serializer.WriteDateTimeOffset(dt);
        }
        public void WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes)
        {
            serializer.WriteBytes(bytes);
        }

        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class?
        {
            serialize.Serialize(value, serializer);
        }
    }

    private sealed class KeySerializer(JsonSerializer _parent) : ISerializer
    {
        internal sealed class KeyNotStringException() : Exception("JSON allows only strings in this location, expected a string.") { }
        public void WriteBool(bool b) => throw new KeyNotStringException();
        public void WriteChar(char c) => throw new KeyNotStringException();
        public void WriteU8(byte b) => throw new KeyNotStringException();
        public void WriteU16(ushort u16) => throw new KeyNotStringException();
        public void WriteU32(uint u32) => throw new KeyNotStringException();
        public void WriteU64(ulong u64) => throw new KeyNotStringException();
        public void WriteI8(sbyte b) => throw new KeyNotStringException();
        public void WriteI16(short i16) => throw new KeyNotStringException();
        public void WriteI32(int i32) => throw new KeyNotStringException();
        public void WriteI64(long i64) => throw new KeyNotStringException();
        public void WriteF32(float f) => throw new KeyNotStringException();
        public void WriteF64(double d) => throw new KeyNotStringException();
        public void WriteDecimal(decimal d) => throw new KeyNotStringException();
        public void WriteDateTime(DateTime dt) => throw new KeyNotStringException();
        public void WriteDateTimeOffset(DateTimeOffset dt) => throw new KeyNotStringException();
        public void WriteBytes(ReadOnlyMemory<byte> bytes) => throw new KeyNotStringException();

        public void WriteString(string s)
        {
            _parent._writer.WritePropertyName(s);
        }

        ITypeSerializer ISerializer.WriteCollection(ISerdeInfo typeInfo, int? size) => throw new KeyNotStringException();
        ITypeSerializer ISerializer.WriteType(ISerdeInfo typeInfo) => throw new KeyNotStringException();
        public void WriteNull() => throw new KeyNotStringException();
    }

    private sealed class DictImpl(JsonSerializer serializer) : ITypeSerializer
    {
        public void End(ISerdeInfo info)
        {
            serializer._writer.WriteEndObject();
        }

        private ISerializer GetSerializer(int index)
            => index % 2 == 0 ? serializer._keySerializer : serializer;

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b) => GetSerializer(index).WriteBool(b);
        public void WriteChar(ISerdeInfo typeInfo, int index, char c) => GetSerializer(index).WriteChar(c);
        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) => GetSerializer(index).WriteDecimal(d);
        public void WriteF32(ISerdeInfo typeInfo, int index, float f) => GetSerializer(index).WriteF32(f);
        public void WriteF64(ISerdeInfo typeInfo, int index, double d) => GetSerializer(index).WriteF64(d);
        public void WriteI16(ISerdeInfo typeInfo, int index, short i16) => GetSerializer(index).WriteI16(i16);
        public void WriteI32(ISerdeInfo typeInfo, int index, int i32) => GetSerializer(index).WriteI32(i32);
        public void WriteI64(ISerdeInfo typeInfo, int index, long i64) => GetSerializer(index).WriteI64(i64);
        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b) => GetSerializer(index).WriteI8(b);
        public void WriteNull(ISerdeInfo typeInfo, int index) => GetSerializer(index).WriteNull();
        public void WriteString(ISerdeInfo typeInfo, int index, string s) => GetSerializer(index).WriteString(s);
        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) => GetSerializer(index).WriteU16(u16);
        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32) => GetSerializer(index).WriteU32(u32);
        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64) => GetSerializer(index).WriteU64(u64);
        public void WriteU8(ISerdeInfo typeInfo, int index, byte b) => GetSerializer(index).WriteU8(b);
        public void WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt) => GetSerializer(index).WriteDateTime(dt);
        public void WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt) => GetSerializer(index).WriteDateTimeOffset(dt);
        public void WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes) => GetSerializer(index).WriteBytes(bytes);
        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class?
            => serialize.Serialize(value, GetSerializer(index));
    }
}