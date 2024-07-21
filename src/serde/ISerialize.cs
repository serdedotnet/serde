namespace Serde;

public interface ISerialize<T> : ISerdeInfoProvider
{
    void Serialize(T value, ISerializer serializer);
}

public interface ISerializeType
{
    void SerializeField<T, U>(ISerdeInfo typeInfo, int index, T value, U serialize) where U : ISerialize<T>;
    void SkipField(ISerdeInfo typeInfo, int index) { }
    void End();
}

public static class ISerializeTypeExt
{
    public static void SerializeField<T, U>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value) where U : struct, ISerialize<T>
    {
        serializeType.SerializeField(typeInfo, index, value, default(U));
    }

    public static void SerializeFieldIfNotNull<T, U>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value,
        U serialize) where U : ISerialize<T>
    {
        if (value is null)
        {
            serializeType.SkipField(typeInfo, index);
        }
        else
        {
            serializeType.SerializeField(typeInfo, index, value, serialize);
        }
    }

    public static void SerializeFieldIfNotNull<T, U>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value) where U : struct, ISerialize<T>
    {
        serializeType.SerializeFieldIfNotNull(typeInfo, index, value, default(U));
    }
}

public interface ISerializeCollection
{
    void SerializeElement<T, U>(T value, U serialize) where U : ISerialize<T>;
    void End(ISerdeInfo typeInfo);
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
    void SerializeEnumValue<T, U>(ISerdeInfo typeInfo, int index, T value, U serialize)
        where T : unmanaged
        where U : ISerialize<T>;

    ISerializeType SerializeType(ISerdeInfo typeInfo);
    ISerializeCollection SerializeCollection(ISerdeInfo typeInfo, int? length);
}