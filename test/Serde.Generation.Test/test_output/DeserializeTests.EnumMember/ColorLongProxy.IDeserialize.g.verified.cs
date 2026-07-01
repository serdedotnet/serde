//HintName: ColorLongProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorLongProxy : Serde.IDeserialize<ColorLong>
{
    ColorLong IDeserialize<ColorLong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        ColorLong _l_result = index switch {
            0 => ColorLong.Red,
            1 => ColorLong.Green,
            2 => ColorLong.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
