﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Serde;

/// <summary>
/// Represents a "serialize" object. A serialize object contains the logic about how to serialize a
/// particular type with an arbitrary serializer. It is usually a singleton object -- almost all
/// types should not need to modify how they are serialized based on some mutable state. The
/// serialize object for a type is usually acquired through an <see cref="ISerializeProvider{T}" />.
///
/// The implementation of this interface is usually generated by the <c>Serde</c> code generator.
/// Most implementations are straightforward based on the layout of the type. For example, a
/// Point(int X, int Y) would be serialized by writing the X and Y values into the serializer:
///
/// <code>
/// public void Serialize(Point value, ISerializer serializer)
/// {
///     var info = SerdeInfoProvider.GetInfo(this);
///     var typeSer = serializer.WriteType(info);
///     typeSer.WriteI32(info, 0, value.X);
///     typeSer.WriteI32(info, 1, value.Y);
///     typeSer.End(info);
/// }
/// </code>
/// </summary>
public interface ISerialize<T> : ISerdeInfoProvider
{
    void Serialize(T value, ISerializer serializer);
}

/// <summary>
/// This is a perf optimization. It allows primitive types (and only primitive types) to be
/// serialized without boxing. It is only useful for serializing collections.
/// </summary>
public interface ITypeSerialize<T>
{
    void Serialize(T value, ITypeSerializer serializer, ISerdeInfo info, int index);
}

public static class TypeSerialize
{
    /// <summary>
    /// Checks if the <typeparamref name="TProvider"/> produces a type that implements <see
    /// cref="ITypeSerialize{T}" />. If it does, it returns that type. Otherwise, it returns a <see
    /// cref="BoxProxy.Ser{T, TProvider}"/>.
    /// </summary>
    public static ITypeSerialize<T> GetOrBox<T, TProvider>()
        where TProvider : ISerializeProvider<T>
    {
        var ser = TProvider.Instance;
        return ser switch
        {
            ITypeSerialize<T> typeSer => typeSer,
            _ => BoxProxy.Ser<T, TProvider>.Instance
        };
    }
}


public interface ISerializeProvider<T>
{
    abstract static ISerialize<T> Instance { get; }
}

public static class SerializeProvider
{
    public static ISerialize<T> GetSerialize<T>() where T : ISerializeProvider<T> => T.Instance;
    public static ISerialize<T> GetSerialize<T, TProvider>()
        where TProvider : ISerializeProvider<T>
        => TProvider.Instance;
}

public interface ISerializer
{
    void WriteBool(bool b);
    void WriteChar(char c);
    void WriteU8(byte b);
    void WriteU16(ushort u16);
    void WriteU32(uint u32);
    void WriteU64(ulong u64);
    void WriteI8(sbyte b);
    void WriteI16(short i16);
    void WriteI32(int i32);
    void WriteI64(long i64);
    void WriteF32(float f);
    void WriteF64(double d);
    void WriteDecimal(decimal d);
    void WriteString(string s);
    void WriteNull();

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

public static class ISerializeExt
{
    public static void WriteValue<T, TProvider>(this ISerializer serializer, T value)
        where TProvider : ISerializeProvider<T>
    {
        var ser = TProvider.Instance;
        ser.Serialize(value, serializer);
    }
    public static void WriteValue<T>(this ISerializer serializer, T value)
        where T : ISerializeProvider<T>
        => serializer.WriteValue<T, T>(value);
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
    void WriteBool(ISerdeInfo typeInfo, int index, bool b);
    void WriteChar(ISerdeInfo typeInfo, int index, char c);
    void WriteU8(ISerdeInfo typeInfo, int index, byte b);
    void WriteU16(ISerdeInfo typeInfo, int index, ushort u16);
    void WriteU32(ISerdeInfo typeInfo, int index, uint u32);
    void WriteU64(ISerdeInfo typeInfo, int index, ulong u64);
    void WriteI8(ISerdeInfo typeInfo, int index, sbyte b);
    void WriteI16(ISerdeInfo typeInfo, int index, short i16);
    void WriteI32(ISerdeInfo typeInfo, int index, int i32);
    void WriteI64(ISerdeInfo typeInfo, int index, long i64);
    void WriteF32(ISerdeInfo typeInfo, int index, float f);
    void WriteF64(ISerdeInfo typeInfo, int index, double d);
    void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d);
    void WriteString(ISerdeInfo typeInfo, int index, string s);
    void WriteNull(ISerdeInfo typeInfo, int index);

    /// <summary>
    /// Write an arbitrary value with custom serialization. For reference types this method may be
    /// used directly. For value types, <see cref="ITypeSerializerExt.WriteBoxedValue{T,
    /// TProvider}(ITypeSerializer, ISerdeInfo, int, T)"/> should be used instead.
    /// </summary>
    void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
        where T : class?;
    void SkipValue(ISerdeInfo typeInfo, int index) { }
    void End(ISerdeInfo info);
}

public static class ITypeSerializerExt
{
    public static void WriteValue<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where T : class?
        where TProvider : ISerializeProvider<T>
        => serializeType.WriteValue(typeInfo, index, value, TProvider.Instance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteStringIfNotNull(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        string? value)
    {
        if (value is null)
        {
            serializeType.SkipValue(typeInfo, index);
        }
        else
        {
            serializeType.WriteString(typeInfo, index, value);
        }
    }

    public static void WriteValueIfNotNull<T>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value,
        ISerialize<T> proxy)
        where T : class?
    {
        if (value is null)
        {
            serializeType.SkipValue(typeInfo, index);
        }
        else
        {
            serializeType.WriteValue(typeInfo, index, value, proxy);
        }
    }

    public static void WriteValueIfNotNull<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where T : class?
        where TProvider : ISerializeProvider<T>
        => serializeType.WriteValueIfNotNull(typeInfo, index, value, TProvider.Instance);

    public static void WriteBoxedValue<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo serdeInfo,
        int index,
        T value)
        where TProvider : ISerializeProvider<T>
    {
        serializeType.WriteValue(serdeInfo, index, value, BoxProxy.Ser<T, TProvider>.Instance);
    }

    public static void WriteBoxedValueIfNotNull<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where TProvider : ISerializeProvider<T>
    {
        if (value is null)
        {
            serializeType.SkipValue(typeInfo, index);
        }
        else
        {
            serializeType.WriteBoxedValue<T, TProvider>(typeInfo, index, value);
        }
    }
}
