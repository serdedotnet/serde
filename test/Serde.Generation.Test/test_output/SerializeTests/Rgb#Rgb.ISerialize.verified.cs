//HintName: Rgb.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct Rgb : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("Rgb", 3);
        type.SerializeField("red"u8, new ByteWrap(this.Red));
        type.SerializeField("green"u8, new ByteWrap(this.Green));
        type.SerializeField("blue"u8, new ByteWrap(this.Blue));
        type.End();
    }
}