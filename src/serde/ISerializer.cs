using System;

namespace Serde;

public interface ISerializer
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
}

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
}
