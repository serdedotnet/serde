//HintName: C2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C2 : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("C2", 1);
        type.SerializeField("map", new DictWrap.SerializeImpl<string, StringWrap, C, IdWrap<C>>(this.Map));
        type.End();
    }
}