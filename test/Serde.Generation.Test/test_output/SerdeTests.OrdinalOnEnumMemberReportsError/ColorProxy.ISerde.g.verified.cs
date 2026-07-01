//HintName: ColorProxy.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class ColorProxy : Serde.ISerde<Color>
{
    void global::Serde.ISerialize<Color>.Serialize(Color value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_index = value switch
        {
            Color.Red => 0,
            Color.Green => 1,
            Color.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'Color'"),
        };
        serializer.WriteEnum(_l_info, _l_index);

    }
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
