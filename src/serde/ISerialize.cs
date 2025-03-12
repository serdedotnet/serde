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
    void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
        where T : class?;
    void SkipValue(ISerdeInfo typeInfo, int index) { }
    void End(ISerdeInfo info);
}

public static class ISerializeTypeExt
{
    public static void WriteValue<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value)
        where T : class
        where TProvider : ISerializeProvider<T>
        => serializeType.WriteValue(typeInfo, index, value, TProvider.SerializeInstance);

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
        => serializeType.WriteValueIfNotNull(typeInfo, index, value, TProvider.SerializeInstance);

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

    public static void WriteBoxedValue<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo serdeInfo,
        int index,
        T value)
        where TProvider : ISerializeProvider<T>
    {
        var proxy = BoxProxy<T, TProvider>.Instance;
        serializeType.WriteValue(serdeInfo, index, value, proxy);
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

    ITypeSerializer WriteType(ISerdeInfo typeInfo);
    ITypeSerializer WriteCollection(ISerdeInfo typeInfo, int? length);
}