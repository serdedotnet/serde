//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, T3, T4, T5> : Serde.ISerialize<S<T1, T2, T3, T4, T5>>
{
    void ISerialize<S<T1, T2, T3, T4, T5>>.Serialize(S<T1, T2, T3, T4, T5> value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 5);
        type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>("fS", value.FS);
        type.SerializeField<T1, IdWrap<T1>>("f1", value.F1);
        type.SerializeFieldIfNotNull<T2, IdWrap<T2>>("f2", value.F2);
        type.SerializeFieldIfNotNull<T3?, Serde.NullableRefWrap.SerializeImpl<T3, IdWrap<T3>>>("f3", value.F3);
        type.SerializeFieldIfNotNull<T4, IdWrap<T4>>("f4", value.F4);
        type.End();
    }
}