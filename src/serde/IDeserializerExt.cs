using System;

namespace Serde;

public static class IDeserializerExt
{
    public static T ReadValue<T, TProvider>(this IDeserializer deserializer)
        where TProvider : IDeserializeProvider<T>
    {
        var de = DeserializeProvider.GetDeserialize<T, TProvider>();
        return de.Deserialize(deserializer);
    }

    public static T ReadValue<T>(this IDeserializer deserializer)
        where T : IDeserializeProvider<T> => deserializer.ReadValue<T, T>();
}

public static class ITypeDeserializerExt
{
    public static T ReadValue<T>(
        this ITypeDeserializer @this,
        ISerdeInfo info,
        int index,
        IDeserialize<T> deserialize
    ) => deserialize.DeserializeAsField(@this, info, index);

    public static T ReadValue<T, TProvider>(
        this ITypeDeserializer @this,
        ISerdeInfo info,
        int index
    )
        where TProvider : IDeserializeProvider<T> =>
        TProvider.Instance.DeserializeAsField(@this, info, index);

    public static Guid ReadGuid(this ITypeDeserializer @this, ISerdeInfo info, int index) =>
        @this.ReadValue(info, index, GuidProxy.Instance);

    [Obsolete("Use ReadValue instead")]
    public static T ReadBoxedValue<T>(
        this ITypeDeserializer deserializeType,
        ISerdeInfo info,
        int index,
        IDeserialize<T> d
    )
        where T : struct
    {
        return (T)deserializeType.ReadValue(info, index, new BoxProxy.De<T>(d))!;
    }

    [Obsolete("Use ReadValue instead")]
    public static T ReadBoxedValue<T, TProvider>(
        this ITypeDeserializer deserializeType,
        ISerdeInfo info,
        int index
    )
        where TProvider : IDeserializeProvider<T>
    {
        return (T)deserializeType.ReadValue(info, index, BoxProxy.De<T, TProvider>.Instance)!;
    }

    [Obsolete("Use ReadValue with an IDeserialize<T> instead")]
    public static T ReadValue<T>(
        this ITypeDeserializer deserializeType,
        ISerdeInfo info,
        int index,
        ITypeDeserialize<T> d
    ) => d.DeserializeAsField(deserializeType, info, index);
}
