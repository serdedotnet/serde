//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial record C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeField("x", new Int32Wrap(this.X));
        type.End();
    }
}