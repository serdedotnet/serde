//HintName: RgbWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial record struct RgbWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var name = Value switch
        {
            Rgb.Red => "red",
            Rgb.Green => "green",
            Rgb.Blue => "blue",
            _ => null
        };
        serializer.SerializeEnumValue("Rgb", name, new Int32Wrap((int)Value));
    }
}