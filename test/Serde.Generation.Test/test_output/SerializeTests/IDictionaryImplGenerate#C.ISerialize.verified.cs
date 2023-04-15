//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeField("rDictionary", new IDictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>(this.RDictionary));
        type.End();
    }
}