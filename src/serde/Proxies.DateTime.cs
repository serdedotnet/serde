
using System;

namespace Serde;

public sealed class DateTimeProxy : ISerdePrimitive<DateTimeProxy, DateTime>
{
    public static DateTimeProxy Instance { get; } = new();
    private DateTimeProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.DateTime", PrimitiveKind.DateTime);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<DateTime>.Serialize(DateTime value, ISerializer serializer)
        => serializer.WriteDateTime(value);
    void ITypeSerialize<DateTime>.Serialize(DateTime value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDateTime(info, index, value);
    DateTime IDeserialize<DateTime>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDateTime();
    DateTime ITypeDeserialize<DateTime>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDateTime(info, index);
}

public sealed class DateTimeOffsetProxy : ISerdePrimitive<DateTimeOffsetProxy, DateTimeOffset>
{
    public static DateTimeOffsetProxy Instance { get; } = new();
    private DateTimeOffsetProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.DateTimeOffset", PrimitiveKind.String);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<DateTimeOffset>.Serialize(DateTimeOffset value, ISerializer serializer)
        => serializer.WriteDateTimeOffset(value);
    DateTimeOffset IDeserialize<DateTimeOffset>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDateTimeOffset();

    void ITypeSerialize<DateTimeOffset>.Serialize(DateTimeOffset value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDateTimeOffset(info, index, value);

    DateTimeOffset ITypeDeserialize<DateTimeOffset>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDateTimeOffset(info, index);
}

public sealed class DateOnlyProxy : ISerdePrimitive<DateOnlyProxy, DateOnly>
{
    public static DateOnlyProxy Instance { get; } = new();
    private DateOnlyProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.DateOnly", PrimitiveKind.DateOnly);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<DateOnly>.Serialize(DateOnly value, ISerializer serializer)
        => serializer.WriteDateOnly(value);
    void ITypeSerialize<DateOnly>.Serialize(DateOnly value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteDateOnly(info, index, value);
    DateOnly IDeserialize<DateOnly>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadDateOnly();
    DateOnly ITypeDeserialize<DateOnly>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadDateOnly(info, index);
}

public sealed class TimeOnlyProxy : ISerdePrimitive<TimeOnlyProxy, TimeOnly>
{
    public static TimeOnlyProxy Instance { get; } = new();
    private TimeOnlyProxy() { }

    public static ISerdeInfo SerdeInfo { get; }
        = Serde.SerdeInfo.MakePrimitive("System.TimeOnly", PrimitiveKind.TimeOnly);
    ISerdeInfo ISerdeInfoProvider.SerdeInfo => SerdeInfo;

    void ISerialize<TimeOnly>.Serialize(TimeOnly value, ISerializer serializer)
        => serializer.WriteTimeOnly(value);
    void ITypeSerialize<TimeOnly>.Serialize(TimeOnly value, ITypeSerializer serializer, ISerdeInfo info, int index)
        => serializer.WriteTimeOnly(info, index, value);
    TimeOnly IDeserialize<TimeOnly>.Deserialize(IDeserializer deserializer)
        => deserializer.ReadTimeOnly();
    TimeOnly ITypeDeserialize<TimeOnly>.Deserialize(ITypeDeserializer deserializer, ISerdeInfo info, int index)
        => deserializer.ReadTimeOnly(info, index);
}
