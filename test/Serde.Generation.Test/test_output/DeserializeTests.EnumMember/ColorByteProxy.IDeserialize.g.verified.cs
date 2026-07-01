//HintName: ColorByteProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorByteProxy : Serde.IDeserialize<ColorByte>
{
    ColorByte IDeserialize<ColorByte>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        ColorByte _l_result = index switch {
            0 => ColorByte.Red,
            1 => ColorByte.Green,
            2 => ColorByte.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
