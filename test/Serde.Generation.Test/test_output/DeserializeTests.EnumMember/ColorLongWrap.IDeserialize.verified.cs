//HintName: ColorLongWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorLongWrap : Serde.IDeserialize<ColorLong>
{
    static ColorLong IDeserialize<ColorLong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = ColorLongSerdeInfo.Instance;
        var de = deserializer.DeserializeType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        }

        return index switch
        {
            0 => ColorLong.Red,
            1 => ColorLong.Green,
            2 => ColorLong.Blue,
            _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
    }
}