using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Serde;

public interface ISerialize<T>
{
    void Serialize(T value, ISerializer serializer);
}

public interface ISerializeProvider<T> : ISerdeInfoProvider
{
    abstract static ISerialize<T> SerializeInstance { get; }
}

public interface ISerializeType
{
    void WriteBool(ISerdeInfo typeInfo, int index, bool b);
    void WriteChar(ISerdeInfo typeInfo, int index, char c);
    void WriteByte(ISerdeInfo typeInfo, int index, byte b);
    void WriteU16(ISerdeInfo typeInfo, int index, ushort u16);
    void WriteU32(ISerdeInfo typeInfo, int index, uint u32);
    void WriteU64(ISerdeInfo typeInfo, int index, ulong u64);
    void WriteSByte(ISerdeInfo typeInfo, int index, sbyte b);
    void WriteI16(ISerdeInfo typeInfo, int index, short i16);
    void WriteI32(ISerdeInfo typeInfo, int index, int i32);
    void WriteI64(ISerdeInfo typeInfo, int index, long i64);
    void WriteFloat(ISerdeInfo typeInfo, int index, float f);
    void WriteDouble(ISerdeInfo typeInfo, int index, double d);
    void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d);
    void WriteString(ISerdeInfo typeInfo, int index, string s);
    void WriteNull(ISerdeInfo typeInfo, int index);
    void WriteField<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
        where T : class?;
    void SkipField(ISerdeInfo typeInfo, int index) { }
    void End(ISerdeInfo info);
}

public static class ISerializeTypeExt
{
    public static void WriteField<T, TProvider>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where T : class
        where TProvider : ISerializeProvider<T>
        => serializeType.WriteField(typeInfo, index, value, TProvider.SerializeInstance);

    public static void WriteFieldIfNotNull<T>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value,
        ISerialize<T> proxy)
        where T : class?
    {
        if (value is null)
        {
            serializeType.SkipField(typeInfo, index);
        }
        else
        {
            serializeType.WriteField(typeInfo, index, value, proxy);
        }
    }

    public static void WriteFieldIfNotNull<T, TProvider>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where T : class?
        where TProvider : ISerializeProvider<T>
        => serializeType.WriteFieldIfNotNull(typeInfo, index, value, TProvider.SerializeInstance);

    private sealed class BoxProxy<T, TProvider> : ISerialize<object?>
        where TProvider : ISerializeProvider<T>
    {
        public static readonly BoxProxy<T, TProvider> Instance = new(TProvider.SerializeInstance);
        private readonly ISerialize<T> _proxy;
        private BoxProxy(ISerialize<T> proxy) { _proxy = proxy; }
        void ISerialize<object?>.Serialize(object? value, ISerializer serializer)
        {
            _proxy.Serialize((T)value!, serializer);
        }
    }

    public static void WriteBoxedField<T, TProvider>(
        this ISerializeType serializeType,
        ISerdeInfo serdeInfo,
        int index,
        T value)
        where TProvider : ISerializeProvider<T>
    {
        var proxy = BoxProxy<T, TProvider>.Instance;
        serializeType.WriteField(serdeInfo, index, value, proxy);
    }

    public static void WriteBoxedFieldIfNotNull<T, TProvider>(
        this ISerializeType serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where TProvider : ISerializeProvider<T>
    {
        if (value is null)
        {
            serializeType.SkipField(typeInfo, index);
        }
        else
        {
            serializeType.WriteBoxedField<T, TProvider>(typeInfo, index, value);
        }
    }
}

public interface ISerializeCollection
{
    void WriteElement<T, U>(T value, U serialize) where U : ISerialize<T>;
    void End(ISerdeInfo typeInfo);
}

public interface ISerializer
{
    void WriteBool(bool b);
    void WriteChar(char c);
    void WriteByte(byte b);
    void WriteU16(ushort u16);
    void WriteU32(uint u32);
    void WriteU64(ulong u64);
    void WriteSByte(sbyte b);
    void WriteI16(short i16);
    void WriteI32(int i32);
    void WriteI64(long i64);
    void WriteFloat(float f);
    void WriteDouble(double d);
    void WriteDecimal(decimal d);
    void WriteString(string s);
    void WriteNull();

    ISerializeType WriteType(ISerdeInfo typeInfo);
    ISerializeCollection WriteCollection(ISerdeInfo typeInfo, int? length);
}