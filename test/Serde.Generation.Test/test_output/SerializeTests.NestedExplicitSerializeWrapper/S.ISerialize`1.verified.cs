//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize<S>
{
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        var _l_typeInfo = SSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayWrap.SerializeImpl<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>>(_l_typeInfo, 0, value.Opts);
        type.End();
    }
}