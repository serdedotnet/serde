//HintName: ColorEnumProxy.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class ColorEnumProxy : Serde.ISerde<ColorEnum>
{
    void global::Serde.ISerialize<ColorEnum>.Serialize(ColorEnum value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_index = value switch
        {
            ColorEnum.Red => 0,
            ColorEnum.Green => 1,
            ColorEnum.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
        };
        serializer.WriteEnum(_l_info, _l_index);

    }
    ColorEnum IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
    {
        var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
        var index = deserializer.ReadEnum(serdeInfo);
        ColorEnum _l_result = index switch {
            0 => ColorEnum.Red,
            1 => ColorEnum.Green,
            2 => ColorEnum.Blue,
            _ => throw new InvalidOperationException($"Unexpected index: {index}")
        };
        return _l_result;
    }
}
