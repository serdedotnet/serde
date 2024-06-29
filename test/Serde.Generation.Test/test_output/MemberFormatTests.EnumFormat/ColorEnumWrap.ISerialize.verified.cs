//HintName: ColorEnumWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorEnumWrap : Serde.ISerialize<ColorEnum>
{
    void ISerialize<ColorEnum>.Serialize(ColorEnum value, ISerializer serializer)
    {
        var name = value switch
        {
            ColorEnum.Red => "Red",
            ColorEnum.Green => "Green",
            ColorEnum.Blue => "Blue",
            _ => null
        };
        serializer.SerializeEnumValue("ColorEnum", name, (int)value, default(Int32Wrap));
    }
}