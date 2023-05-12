//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 2);
        type.SerializeField("one"u8, new Int32Wrap(this.One));
        type.SerializeField("twoWord"u8, new Int32Wrap(this.TwoWord));
        type.End();
    }
}