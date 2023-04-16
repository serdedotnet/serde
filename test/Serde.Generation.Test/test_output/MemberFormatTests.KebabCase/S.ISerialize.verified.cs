//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 2);
        type.SerializeField("one", new Int32Wrap(this.One));
        type.SerializeField("two-word", new Int32Wrap(this.TwoWord));
        type.End();
    }
}