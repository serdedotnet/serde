//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeField("s"u8, new BitVector32SectionWrap(this.S));
        type.End();
    }
}