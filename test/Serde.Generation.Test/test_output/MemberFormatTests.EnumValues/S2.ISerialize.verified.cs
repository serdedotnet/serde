//HintName: S2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S2 : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S2", 1);
        type.SerializeField("E", new ColorEnumWrap(this.E));
        type.End();
    }
}