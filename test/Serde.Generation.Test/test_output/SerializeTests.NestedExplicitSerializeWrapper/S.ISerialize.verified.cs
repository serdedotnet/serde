//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 1);
        type.SerializeField("opts"u8, new Serde.ImmutableArrayWrap.SerializeImpl<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>(this.Opts));
        type.End();
    }
}