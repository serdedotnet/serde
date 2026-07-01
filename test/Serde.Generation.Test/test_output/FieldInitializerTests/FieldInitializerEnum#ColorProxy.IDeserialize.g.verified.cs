//HintName: ColorProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class ColorProxy : Serde.IDeserialize<Color>
{
    Color IDeserialize<Color>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        Color _l_result = index switch {
            0 => Color.Red,
            1 => Color.Green,
            2 => Color.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
