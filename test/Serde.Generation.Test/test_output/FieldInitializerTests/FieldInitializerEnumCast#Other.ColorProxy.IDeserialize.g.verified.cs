//HintName: Other.ColorProxy.IDeserialize.g.cs

#nullable enable

using System;
using Serde;

namespace Other;

partial class ColorProxy : Serde.IDeserialize<Other.Color>
{
    Other.Color IDeserialize<Other.Color>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        Other.Color _l_result = index switch {
            0 => Other.Color.Red,
            1 => Other.Color.Green,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
