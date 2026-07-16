using System;
using System.Threading.Tasks;

namespace Serde;

public sealed class DateTimeProxy : ISerdePrimitive<DateTimeProxy, DateTime>
{
    public static DateTimeProxy Instance { get; } = new();

    private DateTimeProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("System.DateTime", PrimitiveKind.DateTime);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<DateTime>.Serialize(DateTime value, ISerializer serializer) => await serializer.WriteDateTime(value);
#else
    void ISerialize<DateTime>.Serialize(DateTime value, ISerializer serializer) => serializer.WriteDateTime(value);
#endif

#if NET11_0_OR_GREATER
    async Task ISerialize<DateTime>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, DateTime value
    )
    {
        await serializer.WriteDateTime(info, index, value);
    }
#else
    void ISerialize<DateTime>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        DateTime value
    ) => serializer.WriteDateTime(info, index, value);
#endif

    DateTime IDeserialize<DateTime>.Deserialize(IDeserializer deserializer) =>
        deserializer.ReadDateTime();

    DateTime IDeserialize<DateTime>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadDateTime(info, index);
}

public sealed class DateTimeOffsetProxy : ISerdePrimitive<DateTimeOffsetProxy, DateTimeOffset>
{
    public static DateTimeOffsetProxy Instance { get; } = new();

    private DateTimeOffsetProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("System.DateTimeOffset", PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<DateTimeOffset>.Serialize(DateTimeOffset value, ISerializer serializer) => await serializer.WriteDateTimeOffset(value);
#else
    void ISerialize<DateTimeOffset>.Serialize(DateTimeOffset value, ISerializer serializer) => serializer.WriteDateTimeOffset(value);
#endif

    DateTimeOffset IDeserialize<DateTimeOffset>.Deserialize(IDeserializer deserializer) =>
        deserializer.ReadDateTimeOffset();

#if NET11_0_OR_GREATER
    async Task ISerialize<DateTimeOffset>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, DateTimeOffset value
    )
    {
        await serializer.WriteDateTimeOffset(info, index, value);
    }
#else
    void ISerialize<DateTimeOffset>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        DateTimeOffset value
    ) => serializer.WriteDateTimeOffset(info, index, value);
#endif

    DateTimeOffset IDeserialize<DateTimeOffset>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadDateTimeOffset(info, index);
}

public sealed class DateOnlyProxy : ISerdePrimitive<DateOnlyProxy, DateOnly>
{
    public static DateOnlyProxy Instance { get; } = new();

    private DateOnlyProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("System.DateOnly", PrimitiveKind.DateOnly);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<DateOnly>.Serialize(DateOnly value, ISerializer serializer) => await serializer.WriteDateOnly(value);
#else
    void ISerialize<DateOnly>.Serialize(DateOnly value, ISerializer serializer) => serializer.WriteDateOnly(value);
#endif

#if NET11_0_OR_GREATER
    async Task ISerialize<DateOnly>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, DateOnly value
    )
    {
        await serializer.WriteDateOnly(info, index, value);
    }
#else
    void ISerialize<DateOnly>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        DateOnly value
    ) => serializer.WriteDateOnly(info, index, value);
#endif

    DateOnly IDeserialize<DateOnly>.Deserialize(IDeserializer deserializer) =>
        deserializer.ReadDateOnly();

    DateOnly IDeserialize<DateOnly>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadDateOnly(info, index);
}

public sealed class TimeOnlyProxy : ISerdePrimitive<TimeOnlyProxy, TimeOnly>
{
    public static TimeOnlyProxy Instance { get; } = new();

    private TimeOnlyProxy() { }

    public static ISerdeInfo SerdeInfo { get; } =
        Serde.SerdeInfo.MakePrimitive("System.TimeOnly", PrimitiveKind.TimeOnly);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

#if NET11_0_OR_GREATER
    async Task ISerialize<TimeOnly>.Serialize(TimeOnly value, ISerializer serializer) => await serializer.WriteTimeOnly(value);
#else
    void ISerialize<TimeOnly>.Serialize(TimeOnly value, ISerializer serializer) => serializer.WriteTimeOnly(value);
#endif

#if NET11_0_OR_GREATER
    async Task ISerialize<TimeOnly>.SerializeAsField(
        ITypeSerializer serializer, ISerdeInfo info, int index, TimeOnly value
    )
    {
        await serializer.WriteTimeOnly(info, index, value);
    }
#else
    void ISerialize<TimeOnly>.SerializeAsField(
        ITypeSerializer serializer,
        ISerdeInfo info,
        int index,
        TimeOnly value
    ) => serializer.WriteTimeOnly(info, index, value);
#endif

    TimeOnly IDeserialize<TimeOnly>.Deserialize(IDeserializer deserializer) =>
        deserializer.ReadTimeOnly();

    TimeOnly IDeserialize<TimeOnly>.DeserializeAsField(
        ITypeDeserializer deserializer,
        ISerdeInfo info,
        int index
    ) => deserializer.ReadTimeOnly(info, index);
}
