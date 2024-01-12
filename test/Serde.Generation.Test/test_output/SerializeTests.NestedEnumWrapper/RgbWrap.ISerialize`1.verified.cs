//HintName: RgbWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial record struct RgbWrap : Serde.ISerialize<Rgb>
{
    void ISerialize<Rgb>.Serialize(Rgb value, ISerializer serializer)
    {
        var name = value switch
        {
            Rgb.Red => "red",
            Rgb.Green => "green",
            Rgb.Blue => "blue",
            _ => null
        };
        serializer.SerializeEnumValue("Rgb", name, new Int32Wrap((int)value));
    }
}