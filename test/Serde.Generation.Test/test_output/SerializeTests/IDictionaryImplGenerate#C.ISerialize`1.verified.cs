//HintName: C.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize<C>
{
    void ISerialize<C>.Serialize(C value, ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeField<R, Serde.IDictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>>("rDictionary", value.RDictionary);
        type.End();
    }
}