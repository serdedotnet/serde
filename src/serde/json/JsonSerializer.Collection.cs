using System;
using System.Threading.Tasks;

namespace Serde.Json;

partial class JsonSerializer
{
#if NET11_0_OR_GREATER
    private abstract class AsyncCollectionImpl : ITypeSerializer
    {
        protected abstract ISerializer GetSerializer(int index);

        public async Task<ISerializer> WriteFieldStart(ISerdeInfo typeInfo, int index) =>
            GetSerializer(index);
        public async Task WriteFieldEnd(ISerdeInfo typeInfo, int index, ISerializer serializer) { }

        public Task WriteBool(ISerdeInfo info, int index, bool value) => GetSerializer(index).WriteBool(value);
        public Task WriteChar(ISerdeInfo info, int index, char value) => GetSerializer(index).WriteChar(value);
        public Task WriteU8(ISerdeInfo info, int index, byte value) => GetSerializer(index).WriteU8(value);
        public Task WriteU16(ISerdeInfo info, int index, ushort value) => GetSerializer(index).WriteU16(value);
        public Task WriteU32(ISerdeInfo info, int index, uint value) => GetSerializer(index).WriteU32(value);
        public Task WriteU64(ISerdeInfo info, int index, ulong value) => GetSerializer(index).WriteU64(value);
        public Task WriteU128(ISerdeInfo info, int index, UInt128 value) => GetSerializer(index).WriteU128(value);
        public Task WriteI8(ISerdeInfo info, int index, sbyte value) => GetSerializer(index).WriteI8(value);
        public Task WriteI16(ISerdeInfo info, int index, short value) => GetSerializer(index).WriteI16(value);
        public Task WriteI32(ISerdeInfo info, int index, int value) => GetSerializer(index).WriteI32(value);
        public Task WriteI64(ISerdeInfo info, int index, long value) => GetSerializer(index).WriteI64(value);
        public Task WriteI128(ISerdeInfo info, int index, Int128 value) => GetSerializer(index).WriteI128(value);
        public Task WriteF16(ISerdeInfo info, int index, Half value) => GetSerializer(index).WriteF16(value);
        public Task WriteF32(ISerdeInfo info, int index, float value) => GetSerializer(index).WriteF32(value);
        public Task WriteF64(ISerdeInfo info, int index, double value) => GetSerializer(index).WriteF64(value);
        public Task WriteDecimal(ISerdeInfo info, int index, decimal value) => GetSerializer(index).WriteDecimal(value);
        public Task WriteString(ISerdeInfo info, int index, string value) => GetSerializer(index).WriteString(value);
        public Task WriteNull(ISerdeInfo info, int index) => GetSerializer(index).WriteNull();
        public Task WriteDateTime(ISerdeInfo info, int index, DateTime value) => GetSerializer(index).WriteDateTime(value);
        public Task WriteDateTimeOffset(ISerdeInfo info, int index, DateTimeOffset value) => GetSerializer(index).WriteDateTimeOffset(value);
        public Task WriteDateOnly(ISerdeInfo info, int index, DateOnly value) => GetSerializer(index).WriteDateOnly(value);
        public Task WriteTimeOnly(ISerdeInfo info, int index, TimeOnly value) => GetSerializer(index).WriteTimeOnly(value);
        public Task WriteBytes(ISerdeInfo info, int index, ReadOnlyMemory<byte> value) => GetSerializer(index).WriteBytes(value);
        public Task WriteEnum(ISerdeInfo info, int index, ISerdeInfo fieldInfo, int ordinal) => GetSerializer(index).WriteEnum(fieldInfo, ordinal);

        public Task WriteValue<T>(ISerdeInfo info, int index, T value, ISerialize<T> serialize)
            where T : class? =>
            serialize.Serialize(value, GetSerializer(index));

        public abstract Task End(ISerdeInfo info);
    }

    private sealed class EnumerableImpl(JsonSerializer serializer) : AsyncCollectionImpl
    {
        protected override ISerializer GetSerializer(int index) => serializer;

        public override async Task End(ISerdeInfo info)
        {
            serializer._writer.WriteEndArray();
        }
    }

    private sealed class DictImpl(JsonSerializer serializer) : AsyncCollectionImpl
    {
        protected override ISerializer GetSerializer(int index) =>
            index % 2 == 0 ? serializer._keySerializer : serializer;

        public override async Task End(ISerdeInfo info)
        {
            serializer._writer.WriteEndObject();
        }
    }
#else
    private sealed class EnumerableImpl(JsonSerializer serializer) : ITypeSerializer
    {
        public ISerializer WriteFieldStart(ISerdeInfo typeInfo, int fieldIndex)
        {
            return serializer;
        }

        public void WriteFieldEnd(ISerdeInfo typeInfo, int fieldIndex, ISerializer serializer)
        {
            // noop
        }

        public void End(ISerdeInfo info)
        {
            serializer._writer.WriteEndArray();
        }

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b)
        {
            ((ISerializer)serializer).WriteBool(b);
        }

        public void WriteChar(ISerdeInfo typeInfo, int index, char c)
        {
            ((ISerializer)serializer).WriteChar(c);
        }

        public void WriteU8(ISerdeInfo typeInfo, int index, byte b)
        {
            ((ISerializer)serializer).WriteU8(b);
        }

        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) =>
            ((ISerializer)serializer).WriteU16(u16);

        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32)
        {
            ((ISerializer)serializer).WriteU32(u32);
        }

        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
        {
            ((ISerializer)serializer).WriteU64(u64);
        }

        public void WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128)
        {
            ((ISerializer)serializer).WriteU128(u128);
        }

        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
        {
            ((ISerializer)serializer).WriteI8(b);
        }

        public void WriteI16(ISerdeInfo typeInfo, int index, short i16)
        {
            ((ISerializer)serializer).WriteI16(i16);
        }

        public void WriteI32(ISerdeInfo typeInfo, int index, int i32)
        {
            ((ISerializer)serializer).WriteI32(i32);
        }

        public void WriteI64(ISerdeInfo typeInfo, int index, long i64)
        {
            ((ISerializer)serializer).WriteI64(i64);
        }

        public void WriteI128(ISerdeInfo typeInfo, int index, Int128 i128)
        {
            ((ISerializer)serializer).WriteI128(i128);
        }

        public void WriteF16(ISerdeInfo typeInfo, int index, Half h)
        {
            ((ISerializer)serializer).WriteF16(h);
        }

        public void WriteF32(ISerdeInfo typeInfo, int index, float f)
        {
            ((ISerializer)serializer).WriteF32(f);
        }

        public void WriteF64(ISerdeInfo typeInfo, int index, double d)
        {
            ((ISerializer)serializer).WriteF64(d);
        }

        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
        {
            ((ISerializer)serializer).WriteDecimal(d);
        }

        public void WriteString(ISerdeInfo typeInfo, int index, string s)
        {
            ((ISerializer)serializer).WriteString(s);
        }

        public void WriteNull(ISerdeInfo typeInfo, int index)
        {
            ((ISerializer)serializer).WriteNull();
        }

        public void WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt)
        {
            ((ISerializer)serializer).WriteDateTime(dt);
        }

        public void WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt)
        {
            ((ISerializer)serializer).WriteDateTimeOffset(dt);
        }

        public void WriteDateOnly(ISerdeInfo typeInfo, int index, DateOnly d)
        {
            ((ISerializer)serializer).WriteDateOnly(d);
        }

        public void WriteTimeOnly(ISerdeInfo typeInfo, int index, TimeOnly t)
        {
            ((ISerializer)serializer).WriteTimeOnly(t);
        }

        public void WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes)
        {
            ((ISerializer)serializer).WriteBytes(bytes);
        }

        public void WriteEnum(ISerdeInfo typeInfo, int index, ISerdeInfo fieldInfo, int ordinal)
        {
            ((ISerializer)serializer).WriteEnum(fieldInfo, ordinal);
        }

        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class?
        {
            serialize.Serialize(value, serializer);
        }
    }
#endif

#if NET11_0_OR_GREATER
    private sealed class KeySerializer(JsonSerializer parent) : ISerializer
    {
        internal sealed class KeyNotStringException()
            : Exception("JSON allows only strings in this location, expected a string.")
        { }

        private static Task Invalid() => throw new KeyNotStringException();
        private static Task<T> Invalid<T>() => throw new KeyNotStringException();

        public Task WriteBool(bool b) => Invalid();
        public Task WriteChar(char c) => Invalid();
        public Task WriteU8(byte b) => Invalid();
        public Task WriteU16(ushort u16) => Invalid();
        public Task WriteU32(uint u32) => Invalid();
        public Task WriteU64(ulong u64) => Invalid();
        public Task WriteU128(UInt128 u128) => Invalid();
        public Task WriteI8(sbyte b) => Invalid();
        public Task WriteI16(short i16) => Invalid();
        public Task WriteI32(int i32) => Invalid();
        public Task WriteI64(long i64) => Invalid();
        public Task WriteI128(Int128 i128) => Invalid();
        public Task WriteF16(Half h) => Invalid();
        public Task WriteF32(float f) => Invalid();
        public Task WriteF64(double d) => Invalid();
        public Task WriteDecimal(decimal d) => Invalid();
        public Task WriteNull() => Invalid();
        public Task WriteDateTime(DateTime dt) => Invalid();
        public Task WriteDateTimeOffset(DateTimeOffset dt) => Invalid();
        public Task WriteDateOnly(DateOnly d) => Invalid();
        public Task WriteTimeOnly(TimeOnly t) => Invalid();
        public Task WriteBytes(ReadOnlyMemory<byte> bytes) => Invalid();
        public Task WriteEnum(ISerdeInfo info, int ordinal) => Invalid();

        public async Task WriteString(string s)
        {
            parent._writer.WritePropertyName(s);
        }

        Task<ITypeSerializer> ISerializer.WriteCollection(ISerdeInfo typeInfo, int? size) => Invalid<ITypeSerializer>();
        Task<ITypeSerializer> ISerializer.WriteType(ISerdeInfo typeInfo, int fieldCount) => Invalid<ITypeSerializer>();
    }
#else
    private sealed class KeySerializer(JsonSerializer _parent) : ISerializer
    {
        internal sealed class KeyNotStringException()
            : Exception("JSON allows only strings in this location, expected a string.") { }

        public void WriteBool(bool b) => throw new KeyNotStringException();

        public void WriteChar(char c) => throw new KeyNotStringException();

        public void WriteU8(byte b) => throw new KeyNotStringException();

        public void WriteU16(ushort u16) => throw new KeyNotStringException();

        public void WriteU32(uint u32) => throw new KeyNotStringException();

        public void WriteU64(ulong u64) => throw new KeyNotStringException();

        public void WriteU128(UInt128 u128) => throw new KeyNotStringException();

        public void WriteI8(sbyte b) => throw new KeyNotStringException();

        public void WriteI16(short i16) => throw new KeyNotStringException();

        public void WriteI32(int i32) => throw new KeyNotStringException();

        public void WriteI64(long i64) => throw new KeyNotStringException();

        public void WriteI128(Int128 i128) => throw new KeyNotStringException();

        public void WriteF16(Half h) => throw new KeyNotStringException();

        public void WriteF32(float f) => throw new KeyNotStringException();

        public void WriteF64(double d) => throw new KeyNotStringException();

        public void WriteDecimal(decimal d) => throw new KeyNotStringException();

        public void WriteDateTime(DateTime dt) => throw new KeyNotStringException();

        public void WriteDateTimeOffset(DateTimeOffset dt) => throw new KeyNotStringException();

        public void WriteDateOnly(DateOnly d) => throw new KeyNotStringException();

        public void WriteTimeOnly(TimeOnly t) => throw new KeyNotStringException();

        public void WriteBytes(ReadOnlyMemory<byte> bytes) => throw new KeyNotStringException();

        public void WriteString(string s)
        {
            _parent._writer.WritePropertyName(s);
        }

        ITypeSerializer ISerializer.WriteCollection(ISerdeInfo typeInfo, int? size) =>
            throw new KeyNotStringException();

        ITypeSerializer ISerializer.WriteType(ISerdeInfo typeInfo) =>
            throw new KeyNotStringException();

        public void WriteNull() => throw new KeyNotStringException();

        void ISerializer.WriteEnum(ISerdeInfo info, int ordinal) =>
            throw new KeyNotStringException();
    }
#endif

#if !NET11_0_OR_GREATER
    private sealed class DictImpl(JsonSerializer serializer) : ITypeSerializer
    {
        public ISerializer WriteFieldStart(ISerdeInfo info, int index)
        {
            return GetSerializer(index);
        }

        public void WriteFieldEnd(ISerdeInfo info, int index, ISerializer serializer)
        {
            // No-op for JSON
        }

        public void End(ISerdeInfo info)
        {
            serializer._writer.WriteEndObject();
        }

        private ISerializer GetSerializer(int index) =>
            index % 2 == 0 ? serializer._keySerializer : serializer;

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b) =>
            GetSerializer(index).WriteBool(b);

        public void WriteChar(ISerdeInfo typeInfo, int index, char c) =>
            GetSerializer(index).WriteChar(c);

        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) =>
            GetSerializer(index).WriteDecimal(d);

        public void WriteF16(ISerdeInfo typeInfo, int index, Half h) =>
            GetSerializer(index).WriteF16(h);

        public void WriteF32(ISerdeInfo typeInfo, int index, float f) =>
            GetSerializer(index).WriteF32(f);

        public void WriteF64(ISerdeInfo typeInfo, int index, double d) =>
            GetSerializer(index).WriteF64(d);

        public void WriteI16(ISerdeInfo typeInfo, int index, short i16) =>
            GetSerializer(index).WriteI16(i16);

        public void WriteI32(ISerdeInfo typeInfo, int index, int i32) =>
            GetSerializer(index).WriteI32(i32);

        public void WriteI64(ISerdeInfo typeInfo, int index, long i64) =>
            GetSerializer(index).WriteI64(i64);

        public void WriteI128(ISerdeInfo typeInfo, int index, Int128 i128) =>
            GetSerializer(index).WriteI128(i128);

        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b) =>
            GetSerializer(index).WriteI8(b);

        public void WriteNull(ISerdeInfo typeInfo, int index) => GetSerializer(index).WriteNull();

        public void WriteString(ISerdeInfo typeInfo, int index, string s) =>
            GetSerializer(index).WriteString(s);

        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) =>
            GetSerializer(index).WriteU16(u16);

        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32) =>
            GetSerializer(index).WriteU32(u32);

        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64) =>
            GetSerializer(index).WriteU64(u64);

        public void WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128) =>
            GetSerializer(index).WriteU128(u128);

        public void WriteU8(ISerdeInfo typeInfo, int index, byte b) =>
            GetSerializer(index).WriteU8(b);

        public void WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt) =>
            GetSerializer(index).WriteDateTime(dt);

        public void WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt) =>
            GetSerializer(index).WriteDateTimeOffset(dt);

        public void WriteDateOnly(ISerdeInfo typeInfo, int index, DateOnly d) =>
            GetSerializer(index).WriteDateOnly(d);

        public void WriteTimeOnly(ISerdeInfo typeInfo, int index, TimeOnly t) =>
            GetSerializer(index).WriteTimeOnly(t);

        public void WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes) =>
            GetSerializer(index).WriteBytes(bytes);

        public void WriteEnum(ISerdeInfo typeInfo, int index, ISerdeInfo fieldInfo, int ordinal) =>
            GetSerializer(index).WriteEnum(fieldInfo, ordinal);

        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class? => serialize.Serialize(value, GetSerializer(index));
    }
#endif
}
