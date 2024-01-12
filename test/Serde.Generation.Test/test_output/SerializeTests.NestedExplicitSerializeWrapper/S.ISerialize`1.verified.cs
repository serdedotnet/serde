//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize<S>
{
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 1);
        type.SerializeField<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayWrap.SerializeImpl<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>>("opts", value.Opts);
        type.End();
    }
}