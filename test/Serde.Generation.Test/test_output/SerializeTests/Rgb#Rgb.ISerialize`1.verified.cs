//HintName: Rgb.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct Rgb : Serde.ISerialize<Rgb>
{
    void ISerialize<Rgb>.Serialize(Rgb value, ISerializer serializer)
    {
        var type = serializer.SerializeType("Rgb", 3);
        type.SerializeField<byte, ByteWrap>("red", value.Red);
        type.SerializeField<byte, ByteWrap>("green", value.Green);
        type.SerializeField<byte, ByteWrap>("blue", value.Blue);
        type.End();
    }
}