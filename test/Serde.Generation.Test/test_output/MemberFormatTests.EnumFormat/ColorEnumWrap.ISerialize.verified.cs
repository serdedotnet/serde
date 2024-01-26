//HintName: ColorEnumWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial record struct ColorEnumWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var name = Value switch
        {
            ColorEnum.Red => "Red",
            ColorEnum.Green => "Green",
            ColorEnum.Blue => "Blue",
            _ => null
        };
        serializer.SerializeEnumValue("ColorEnum", name, new Int32Wrap((int)Value));
    }
}