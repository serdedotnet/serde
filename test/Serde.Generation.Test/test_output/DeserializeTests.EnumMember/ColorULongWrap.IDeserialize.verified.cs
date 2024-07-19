//HintName: ColorULongWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorULongWrap : Serde.IDeserialize<ColorULong>
{
    static ColorULong IDeserialize<ColorULong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorULongWrap>();
        var de = deserializer.DeserializeType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        }

        return index switch
        {
            0 => ColorULong.Red,
            1 => ColorULong.Green,
            2 => ColorULong.Blue,
            _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
    }
}