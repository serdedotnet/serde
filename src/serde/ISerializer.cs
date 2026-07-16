using System;
using System.Threading.Tasks;

namespace Serde;

#if NET11_0_OR_GREATER
public interface ISerializer : IAsyncDisposable
{
    Task WriteBool(bool b);
    Task WriteChar(char c);
    Task WriteU8(byte b);
    Task WriteU16(ushort u16);
    Task WriteU32(uint u32);
    Task WriteU64(ulong u64);
    Task WriteU128(UInt128 u128);
    Task WriteI8(sbyte b);
    Task WriteI16(short i16);
    Task WriteI32(int i32);
    Task WriteI64(long i64);
    Task WriteI128(Int128 i128);
    Task WriteF16(Half h)
    {
        // Default implementation: promote to float
        return WriteF32((float)h);
    }
    Task WriteF32(float f);
    Task WriteF64(double d);
    Task WriteDecimal(decimal d);
    Task WriteString(string s);
    Task WriteNull();
    Task WriteDateTime(DateTime dt);
    Task WriteDateTimeOffset(DateTimeOffset dt);
    Task WriteBytes(ReadOnlyMemory<byte> bytes);
    Task WriteDateOnly(DateOnly d)
    {
        // Default implementation: serialize as ISO 8601 date string
        return WriteString(d.ToString("yyyy-MM-dd"));
    }
    Task WriteTimeOnly(TimeOnly t)
    {
        // Default implementation: serialize as ISO 8601 time string
        return WriteString(t.ToString("HH:mm:ss"));
    }
    Task WriteEnum(ISerdeInfo info, int ordinal);

    /// <summary>
    /// Write a collection type -- either a list or a dictionary.
    /// </summary>
    /// <param name="info">
    /// The type information. The <see cref="ISerdeInfo.Kind"/> must be either <see
    /// cref="InfoKind.List"/> or <see cref="InfoKind.Dictionary" />.
    /// </param>
    /// <param name="count">
    /// The size of the type. This is the number of elements. This parameter may be null if the size
    /// is not known at the call site, but certain formats may not support types of unknown size and
    /// throw <see cref="NotSupportedException" />.
    /// </param>
    /// <returns>
    /// An <see cref="ITypeSerializer" /> that can be used to serialize the type. After this method
    /// is called, the retuned <see cref="ITypeSerializer" /> should be used to serialize the type.
    /// The <see cref="ITypeSerializer.End" /> method should be called when the type is fully
    /// serialized. The parent <see cref="ISerializer"/> should not be used after this method is
    /// called, until the <see cref="ITypeSerializer.End" /> method is called. Before the <see
    /// cref="ITypeSerializer.End" /> method is called, all operations on the parent <see
    /// cref="ISerializer" /> have undefined behavior.
    /// </returns>
    Task<ITypeSerializer> WriteCollection(ISerdeInfo info, int? count);

    /// <summary>
    /// Write a non-collection, non-primitive type. This could be a custom type, an enum, a union, a
    /// nullable type, etc. The full set of options corresponds to the options represented by <see
    /// cref="ISerdeInfo" />. The <paramref name="fieldCount"/> parameter is used to indicate the
    /// number of nested fields to serialize. If some fields are skipped, the <paramref
    /// name="fieldCount"/> parameter should be set to the number of fields that will be serialized.
    /// If all fields are serialized, the <paramref name="fieldCount"/> parameter should be equal to
    /// <see cref="ISerdeInfo.FieldCount"/>.
    /// </summary>
    /// <returns>
    /// An <see cref="ITypeSerializer" /> that can be used to serialize the type. After this method
    /// is called, the retuned <see cref="ITypeSerializer" /> should be used to serialize the type.
    /// The <see cref="ITypeSerializer.End" /> method should be called when the type is fully
    /// serialized. The parent <see cref="ISerializer"/> should not be used after this method is
    /// called, until the <see cref="ITypeSerializer.End" /> method is called. Before the <see
    /// cref="ITypeSerializer.End" /> method is called, all operations on the parent <see
    /// cref="ISerializer" /> have undefined behavior.
    /// </returns>
    Task<ITypeSerializer> WriteType(ISerdeInfo info, int fieldCount);

    async ValueTask IAsyncDisposable.DisposeAsync() { }
}
#else
public interface ISerializer : IDisposable
{
    void WriteBool(bool b);
    void WriteChar(char c);
    void WriteU8(byte b);
    void WriteU16(ushort u16);
    void WriteU32(uint u32);
    void WriteU64(ulong u64);
    void WriteU128(UInt128 u128);
    void WriteI8(sbyte b);
    void WriteI16(short i16);
    void WriteI32(int i32);
    void WriteI64(long i64);
    void WriteI128(Int128 i128);
    void WriteF16(Half h)
    {
        // Default implementation: promote to float
        WriteF32((float)h);
    }
    void WriteF32(float f);
    void WriteF64(double d);
    void WriteDecimal(decimal d);
    void WriteString(string s);
    void WriteNull();
    void WriteDateTime(DateTime dt);
    void WriteDateTimeOffset(DateTimeOffset dt);
    void WriteBytes(ReadOnlyMemory<byte> bytes);
    void WriteDateOnly(DateOnly d)
    {
        // Default implementation: serialize as ISO 8601 date string
        WriteString(d.ToString("yyyy-MM-dd"));
    }
    void WriteTimeOnly(TimeOnly t)
    {
        // Default implementation: serialize as ISO 8601 time string
        WriteString(t.ToString("HH:mm:ss"));
    }
    void WriteEnum(ISerdeInfo info, int ordinal);

    /// <summary>
    /// Write a collection type -- either a list or a dictionary.
    /// </summary>
    /// <param name="info">
    /// The type information. The <see cref="ISerdeInfo.Kind"/> must be either <see
    /// cref="InfoKind.List"/> or <see cref="InfoKind.Dictionary" />.
    /// </param>
    /// <param name="count">
    /// The size of the type. This is the number of elements. This parameter may be null if the size
    /// is not known at the call site, but certain formats may not support types of unknown size and
    /// throw <see cref="NotSupportedException" />.
    /// </param>
    /// <returns>
    /// An <see cref="ITypeSerializer" /> that can be used to serialize the type. After this method
    /// is called, the retuned <see cref="ITypeSerializer" /> should be used to serialize the type.
    /// The <see cref="ITypeSerializer.End" /> method should be called when the type is fully
    /// serialized. The parent <see cref="ISerializer"/> should not be used after this method is
    /// called, until the <see cref="ITypeSerializer.End" /> method is called. Before the <see
    /// cref="ITypeSerializer.End" /> method is called, all operations on the parent <see
    /// cref="ISerializer" /> have undefined behavior.
    /// </returns>
    ITypeSerializer WriteCollection(ISerdeInfo info, int? count);

    /// <summary>
    /// Write a non-collection, non-primitive type. This could be a custom type, an enum, a union, a
    /// nullable type, etc. The full set of options corresponds to the options represented by <see
    /// cref="ISerdeInfo" />.
    /// </summary>
    /// <returns>
    /// An <see cref="ITypeSerializer" /> that can be used to serialize the type. After this method
    /// is called, the retuned <see cref="ITypeSerializer" /> should be used to serialize the type.
    /// The <see cref="ITypeSerializer.End" /> method should be called when the type is fully
    /// serialized. The parent <see cref="ISerializer"/> should not be used after this method is
    /// called, until the <see cref="ITypeSerializer.End" /> method is called. Before the <see
    /// cref="ITypeSerializer.End" /> method is called, all operations on the parent <see
    /// cref="ISerializer" /> have undefined behavior.
    /// </returns>
    ITypeSerializer WriteType(ISerdeInfo info);

    /// <summary>
    /// Write a non-collection, non-primitive type. This could be a custom type, an enum, a union, a
    /// nullable type, etc. The full set of options corresponds to the options represented by <see
    /// cref="ISerdeInfo" />. The <paramref name="fieldCount"/> parameter is used to indicate the
    /// number of nested fields to serialize. If some fields are skipped, the <paramref
    /// name="fieldCount"/> parameter should be set to the number of fields that will be serialized.
    /// If all fields are serialized, the <paramref name="fieldCount"/> parameter should be equal to
    /// <see cref="ISerdeInfo.FieldCount"/>.
    /// </summary>
    /// <returns>
    /// An <see cref="ITypeSerializer" /> that can be used to serialize the type. After this method
    /// is called, the retuned <see cref="ITypeSerializer" /> should be used to serialize the type.
    /// The <see cref="ITypeSerializer.End" /> method should be called when the type is fully
    /// serialized. The parent <see cref="ISerializer"/> should not be used after this method is
    /// called, until the <see cref="ITypeSerializer.End" /> method is called. Before the <see
    /// cref="ITypeSerializer.End" /> method is called, all operations on the parent <see
    /// cref="ISerializer" /> have undefined behavior.
    /// </returns>
    ITypeSerializer WriteType(ISerdeInfo info, int fieldCount)
    {
        // Default implementation: formats whose serialized representation does not need to know the
        // number of fields up front (such as JSON, which delimits objects structurally) can ignore
        // the field count and defer to the single-argument overload. Formats that must write the
        // field count as part of the type header should override this method.
        return WriteType(info);
    }

    void IDisposable.Dispose() { }
}
#endif

/// <summary>
/// This interface is used to serialize non-primitive types. All non-primitive types are
/// aggregates of other types. For custom types (structs and classes) these are member fields.
/// For collections these are the elements in collections. For enums, this is the underlying value
/// inside the enum.
///
/// The operations on <see cref="ITypeSerializer"/> are used to serialize
/// each of the contained types. The <see cref="ITypeSerializer.End"/> method should be called when the
/// type is fully serialized.
/// </summary>
public interface ITypeSerializer
{
#if NET11_0_OR_GREATER
    Task<ISerializer> WriteFieldStart(ISerdeInfo typeInfo, int index);
    Task WriteFieldEnd(ISerdeInfo typeInfo, int index, ISerializer serializer);

    Task WriteBool(ISerdeInfo typeInfo, int index, bool b);
    Task WriteChar(ISerdeInfo typeInfo, int index, char c);
    Task WriteU8(ISerdeInfo typeInfo, int index, byte b);
    Task WriteU16(ISerdeInfo typeInfo, int index, ushort u16);
    Task WriteU32(ISerdeInfo typeInfo, int index, uint u32);
    Task WriteU64(ISerdeInfo typeInfo, int index, ulong u64);
    Task WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128);
    Task WriteI8(ISerdeInfo typeInfo, int index, sbyte b);
    Task WriteI16(ISerdeInfo typeInfo, int index, short i16);
    Task WriteI32(ISerdeInfo typeInfo, int index, int i32);
    Task WriteI64(ISerdeInfo typeInfo, int index, long i64);
    Task WriteI128(ISerdeInfo typeInfo, int index, Int128 i128);
    Task WriteF16(ISerdeInfo typeInfo, int index, Half h) =>
        WriteF32(typeInfo, index, (float)h);
    Task WriteF32(ISerdeInfo typeInfo, int index, float f);
    Task WriteF64(ISerdeInfo typeInfo, int index, double d);
    Task WriteDecimal(ISerdeInfo typeInfo, int index, decimal d);
    Task WriteString(ISerdeInfo typeInfo, int index, string s);
    Task WriteNull(ISerdeInfo typeInfo, int index);
    Task WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt);
    Task WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt);
    Task WriteDateOnly(ISerdeInfo typeInfo, int index, DateOnly d) =>
        WriteString(typeInfo, index, d.ToString("yyyy-MM-dd"));
    Task WriteTimeOnly(ISerdeInfo typeInfo, int index, TimeOnly t) =>
        WriteString(typeInfo, index, t.ToString("HH:mm:ss"));
    Task WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes);
    Task WriteEnum(ISerdeInfo typeInfo, int index, ISerdeInfo fieldInfo, int ordinal);

    Task WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
        where T : class?;

    async Task SkipValue(ISerdeInfo typeInfo, int index) { }
    Task End(ISerdeInfo info);

    // Non-virtual/abstract members
    public sealed Task WriteValue<T, TProvider>(ISerdeInfo typeInfo, int index, T value)
        where T : class?
        where TProvider : ISerializeProvider<T> =>
        TProvider.Instance.SerializeAsField(this, typeInfo, index, value);
#else
    ISerializer WriteFieldStart(ISerdeInfo typeInfo, int index);
    void WriteFieldEnd(ISerdeInfo typeInfo, int index, ISerializer serializer);

    void WriteBool(ISerdeInfo typeInfo, int index, bool b);
    void WriteChar(ISerdeInfo typeInfo, int index, char c);
    void WriteU8(ISerdeInfo typeInfo, int index, byte b);
    void WriteU16(ISerdeInfo typeInfo, int index, ushort u16);
    void WriteU32(ISerdeInfo typeInfo, int index, uint u32);
    void WriteU64(ISerdeInfo typeInfo, int index, ulong u64);
    void WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128);
    void WriteI8(ISerdeInfo typeInfo, int index, sbyte b);
    void WriteI16(ISerdeInfo typeInfo, int index, short i16);
    void WriteI32(ISerdeInfo typeInfo, int index, int i32);
    void WriteI64(ISerdeInfo typeInfo, int index, long i64);
    void WriteI128(ISerdeInfo typeInfo, int index, Int128 i128);
    void WriteF16(ISerdeInfo typeInfo, int index, Half h)
    {
        // Default implementation: promote to float
        WriteF32(typeInfo, index, (float)h);
    }
    void WriteF32(ISerdeInfo typeInfo, int index, float f);
    void WriteF64(ISerdeInfo typeInfo, int index, double d);
    void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d);
    void WriteString(ISerdeInfo typeInfo, int index, string s);
    void WriteNull(ISerdeInfo typeInfo, int index);
    void WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt);
    void WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt);
    void WriteDateOnly(ISerdeInfo typeInfo, int index, DateOnly d)
    {
        // Default implementation: serialize as ISO 8601 date string
        WriteString(typeInfo, index, d.ToString("yyyy-MM-dd"));
    }
    void WriteTimeOnly(ISerdeInfo typeInfo, int index, TimeOnly t)
    {
        // Default implementation: serialize as ISO 8601 time string
        WriteString(typeInfo, index, t.ToString("HH:mm:ss"));
    }
    void WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes);

    /// <summary>
    /// Write an enum value.
    /// </summary>
    /// <param name="typeInfo">
    /// The type information for the parent type.
    /// </param>
    /// <param name="index">
    /// The index of the current field in the parent type.
    /// </param>
    /// <param name="fieldInfo">
    /// The type information for the enum field.
    /// </param>
    /// <param name="ordinal">
    /// The ordinal value of the enum. This is the index of the enum variant in the enum type.
    /// </param>
    void WriteEnum(ISerdeInfo typeInfo, int index, ISerdeInfo fieldInfo, int ordinal);

    /// <summary>
    /// Write an arbitrary value with custom serialization. For reference types this method may be
    /// used directly.
    /// </summary>
    /// <remarks>
    /// This method only accepts reference types to avoid code size explosion in AOT. For value types,
    /// use <see cref="ITypeSerializerExt.WriteValue{T}(ITypeSerializer, ISerdeInfo, int, T, ISerialize{T})" /> or
    /// <see cref="ITypeSerializerExt.WriteValue{T, TProvider}(ITypeSerializer, ISerdeInfo, int, T)" />.
    /// </remarks>
    void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
        where T : class?;

    void SkipValue(ISerdeInfo typeInfo, int index) { }
    void End(ISerdeInfo info);

    // Non-virtual/abstract members
    public sealed void WriteValue<T, TProvider>(ISerdeInfo typeInfo, int index, T value)
        where T : class?
        where TProvider : ISerializeProvider<T>
    {
        TProvider.Instance.SerializeAsField(this, typeInfo, index, value);
    }
#endif
}
