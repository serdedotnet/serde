//HintName: ColorIntProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorIntProxy : Serde.IDeserialize<ColorInt>
{
    ColorInt IDeserialize<ColorInt>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        ColorInt _l_result = index switch {
            0 => ColorInt.Red,
            1 => ColorInt.Green,
            2 => ColorInt.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
