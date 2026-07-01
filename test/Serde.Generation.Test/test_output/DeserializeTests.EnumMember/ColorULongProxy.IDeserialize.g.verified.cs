//HintName: ColorULongProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorULongProxy : Serde.IDeserialize<ColorULong>
{
    ColorULong IDeserialize<ColorULong>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        ColorULong _l_result = index switch {
            0 => ColorULong.Red,
            1 => ColorULong.Green,
            2 => ColorULong.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
