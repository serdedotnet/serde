using System;
using System.Runtime.CompilerServices;

namespace Serde;

public static class ISerializerExt
{
    public static void WriteValue<T, TProvider>(this ISerializer serializer, T value)
        where TProvider : ISerializeProvider<T>
    {
        var ser = TProvider.Instance;
        ser.Serialize(value, serializer);
    }

    public static void WriteValue<T>(this ISerializer serializer, T value)
        where T : ISerializeProvider<T> => serializer.WriteValue<T, T>(value);
}

public static class ISerializeExt
{
    [Obsolete($"Use {nameof(ISerializerExt)}.{nameof(ISerializerExt.WriteValue)} instead")]
    public static void WriteValue<T, TProvider>(ISerializer serializer, T value)
        where TProvider : ISerializeProvider<T>
    {
        var ser = TProvider.Instance;
        ser.Serialize(value, serializer);
    }

    [Obsolete($"Use {nameof(ISerializerExt)}.{nameof(ISerializerExt.WriteValue)} instead")]
    public static void WriteValue<T>(ISerializer serializer, T value)
        where T : ISerializeProvider<T> => serializer.WriteValue<T, T>(value);
}

public static class ITypeSerializerExt
{
    public static void WriteValue<T>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value,
        ISerialize<T> proxy
    ) => proxy.SerializeAsField(serializeType, typeInfo, index, value);

    public static void WriteValue<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value
    )
        where TProvider : ISerializeProvider<T> =>
        TProvider.Instance.SerializeAsField(serializeType, typeInfo, index, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteStringIfNotNull(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        string? value
    )
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
        T? value,
        ISerialize<T?> proxy
    )
        where T : class
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
        T? value
    )
        where T : class
        where TProvider : ISerializeProvider<T?> =>
        serializeType.WriteValueIfNotNull(typeInfo, index, value, TProvider.Instance);

    public static void WriteValueIfNotNull<T>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T? value,
        ISerialize<T?> proxy
    )
        where T : struct
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
        T? value
    )
        where T : struct
        where TProvider : ISerializeProvider<T?> =>
        serializeType.WriteValueIfNotNull(typeInfo, index, value, TProvider.Instance);

    public static void WriteGuid(
        this ITypeSerializer typeSerializer,
        ISerdeInfo serdeInfo,
        int index,
        Guid value
    ) => typeSerializer.WriteValue(serdeInfo, index, value, GuidProxy.Instance);

    [Obsolete("Use WriteValue instead")]
    public static void WriteBoxedValue<T>(
        this ITypeSerializer serializeType,
        ISerdeInfo serdeInfo,
        int index,
        T value,
        ISerialize<T> proxy
    )
        where T : struct
    {
        serializeType.WriteValue(serdeInfo, index, value, new BoxProxy.Ser<T>(proxy));
    }

    [Obsolete("Use WriteValue instead")]
    public static void WriteBoxedValue<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo serdeInfo,
        int index,
        T value
    )
        where TProvider : ISerializeProvider<T>
    {
        serializeType.WriteValue(serdeInfo, index, value, BoxProxy.Ser<T, TProvider>.Instance);
    }

    [Obsolete("Use WriteValue instead")]
    public static void WriteBoxedValueIfNotNull<T, TProvider>(
        this ITypeSerializer serializeType,
        ISerdeInfo typeInfo,
        int index,
        T value
    )
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

    [Obsolete("Use WriteValue with an ISerialize<T> instead")]
    public static void WriteValue<T>(
        this ITypeSerializer serializeType,
        ISerdeInfo serdeInfo,
        int index,
        T value,
        ITypeSerialize<T> proxy
    ) => proxy.SerializeAsField(serializeType, serdeInfo, index, value);
}
