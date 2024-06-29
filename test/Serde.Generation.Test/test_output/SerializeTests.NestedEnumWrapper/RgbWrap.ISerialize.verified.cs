//HintName: RgbWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct RgbWrap : Serde.ISerialize<Rgb>
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
        serializer.SerializeEnumValue("Rgb", name, (int)value, default(Int32Wrap));
    }
}