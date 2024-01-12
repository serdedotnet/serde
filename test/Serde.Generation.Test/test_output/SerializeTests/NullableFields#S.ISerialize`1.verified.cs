//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, TSerialize> : Serde.ISerialize<S<T1, T2, TSerialize>>
{
    void ISerialize<S<T1, T2, TSerialize>>.Serialize(S<T1, T2, TSerialize> value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 4);
        type.SerializeFieldIfNotNull<int?, Serde.NullableWrap.SerializeImpl<int, Int32Wrap>>("fI", value.FI);
        type.SerializeFieldIfNotNull<TSerialize?, Serde.NullableWrap.SerializeImpl<TSerialize, IdWrap<TSerialize>>>("f3", value.F3);
        type.End();
    }
}