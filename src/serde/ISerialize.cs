using System;
using System.Text;

namespace Serde;

public interface ISerialize
{
    void Serialize(ISerializer serializer);
}

public interface ISerialize<T>
{
    void Serialize(T value, ISerializer serializer);
}

public interface ISerializeType
{
    void SerializeField<T>(string name, T value) where T : ISerialize
    {
        SerializeField(Encoding.UTF8.GetBytes(name), value);
    }
    void SerializeField<T>(Utf8Span name, T value) where T : ISerialize;
    void SerializeField<T>(string name, T value, ReadOnlySpan<Attribute> attributes) where T : ISerialize
        => SerializeField(name, value);
    void SerializeField<T>(Utf8Span name, T value, ReadOnlySpan<Attribute> attributes) where T : ISerialize
        => SerializeField(name, value);
    void SkipField(string name) { SkipField(Encoding.UTF8.GetBytes(name)); }
    void SkipField(Utf8Span name) { }

    void SerializeField<T, U>(string name, T value, U serialize) where U : ISerialize<T>;
    void End();
}

public static class ISerializeTypeExt
{
    public static void SerializeFieldIfNotNull<T, U>(
        this ISerializeType serializeType,
        string name,
        T value,
        U rawValue) where T : ISerialize
        => SerializeFieldIfNotNull(serializeType, name, value, rawValue, ReadOnlySpan<Attribute>.Empty);

    public static void SerializeFieldIfNotNull<T, U>(
        this ISerializeType serializeType,
        Utf8Span name,
        T value,
        U rawValue) where T : ISerialize
        => SerializeFieldIfNotNull(serializeType, name, value, rawValue, ReadOnlySpan<Attribute>.Empty);

    public static void SerializeFieldIfNotNull<T, U>(
        this ISerializeType serializeType,
        string name,
        T value,
        U rawValue,
        ReadOnlySpan<Attribute> attributes) where T : ISerialize
    {
        if (rawValue is null)
        {
            serializeType.SkipField(name);
        }
        else
        {
            serializeType.SerializeField(name, value, attributes);
        }
    }

    public static void SerializeFieldIfNotNull<T, U>(
        this ISerializeType serializeType,
        Utf8Span name,
        T value,
        U rawValue,
        ReadOnlySpan<Attribute> attributes) where T : ISerialize
    {
        if (rawValue is null)
        {
            serializeType.SkipField(name);
        }
        else
        {
            serializeType.SerializeField(name, value, attributes);
        }
    }
}

public interface ISerializeEnumerable
{
    void SerializeElement<T>(T value) where T : ISerialize;
    void SerializeElement<T, U>(T value, U serialize) where U : ISerialize<T>;
    void End();
}

public interface ISerializeDictionary
{
    void SerializeKey<T>(T key) where T : ISerialize;
    void SerializeKey<T, U>(T key, U serialize) where U : ISerialize<T>;
    void SerializeValue<T>(T value) where T : ISerialize;
    void SerializeValue<T, U>(T value, U serialize) where U : ISerialize<T>;
    void End();
}

public interface ISerializer
{
    void SerializeBool(bool b);
    void SerializeChar(char c);
    void SerializeByte(byte b);
    void SerializeU16(ushort u16);
    void SerializeU32(uint u32);
    void SerializeU64(ulong u64);
    void SerializeSByte(sbyte b);
    void SerializeI16(short i16);
    void SerializeI32(int i32);
    void SerializeI64(long i64);
    void SerializeFloat(float f);
    void SerializeDouble(double d);
    void SerializeDecimal(decimal d);
    void SerializeString(string s);
    void SerializeNull();
    void SerializeNotNull<T>(T t) where T : notnull, ISerialize;
    void SerializeNotNull<T, U>(T t, U u)
        where T : notnull
        where U : ISerialize<T>;
    void SerializeEnumValue<T>(string enumName, string? valueName, T value) where T : notnull, ISerialize;
    void SerializeEnumValue<T, U>(string enumName, string? valueName, T value, U serialize)
        where T : struct, Enum
        where U : ISerialize<T>;

    ISerializeType SerializeType(string name, int numFields);
    ISerializeEnumerable SerializeEnumerable(string typeName, int? length);
    ISerializeDictionary SerializeDictionary(int? length);
}