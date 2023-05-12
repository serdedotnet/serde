//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 1);
        type.SerializeField("e"u8, new ColorEnumWrap(this.E));
        type.End();
    }
}