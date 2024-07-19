//HintName: ColorEnumWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorEnumWrap : Serde.IDeserialize<ColorEnum>
{
    static ColorEnum IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>();
        var de = deserializer.DeserializeType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
        }

        return index switch
        {
            0 => ColorEnum.Red,
            1 => ColorEnum.Green,
            2 => ColorEnum.Blue,
            _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
    }
}