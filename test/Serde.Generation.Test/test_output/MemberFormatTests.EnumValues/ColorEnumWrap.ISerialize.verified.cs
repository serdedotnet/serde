//HintName: ColorEnumWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorEnumWrap : Serde.ISerialize<ColorEnum>
{
    void ISerialize<ColorEnum>.Serialize(ColorEnum value, ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>();
        var index = value switch
        {
            ColorEnum.Red => 0,
            ColorEnum.Green => 1,
            ColorEnum.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
        };
        serializer.SerializeEnumValue(_l_serdeInfo, index, (int)value, default(global::Serde.Int32Wrap));
    }
}