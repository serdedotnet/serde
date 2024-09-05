//HintName: ColorEnumWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorEnumWrap : Serde.IDeserialize<ColorEnum>
{
    static ColorEnum IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>();
        var de = deserializer.ReadType(serdeInfo);
        int index;
        if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
        {
            throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
        }

        return index switch
        {
            0 => ColorEnum.Red,
            1 => ColorEnum.Green,
            2 => ColorEnum.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")};
    }
}