//HintName: C2.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class C2 : Serde.ISerialize<C2>
{
    void ISerialize<C2>.Serialize(C2 value, ISerializer serializer)
    {
        var type = serializer.SerializeType("C2", 1);
        type.SerializeField<System.Collections.Generic.Dictionary<string, C>, Serde.DictWrap.SerializeImpl<string, StringWrap, C, IdWrap<C>>>("map", value.Map);
        type.End();
    }
}