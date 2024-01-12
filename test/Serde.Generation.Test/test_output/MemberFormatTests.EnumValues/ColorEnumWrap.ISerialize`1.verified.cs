//HintName: ColorEnumWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial record struct ColorEnumWrap : Serde.ISerialize<ColorEnum>
{
    void ISerialize<ColorEnum>.Serialize(ColorEnum value, ISerializer serializer)
    {
        var name = value switch
        {
            ColorEnum.Red => "red",
            ColorEnum.Green => "green",
            ColorEnum.Blue => "blue",
            _ => null
        };
        serializer.SerializeEnumValue("ColorEnum", name, new Int32Wrap((int)value));
    }
}