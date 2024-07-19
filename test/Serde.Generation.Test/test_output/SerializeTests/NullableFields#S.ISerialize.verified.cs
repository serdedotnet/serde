//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, TSerialize> : Serde.ISerialize<S<T1, T2, TSerialize>>
{
    void ISerialize<S<T1, T2, TSerialize>>.Serialize(S<T1, T2, TSerialize> value, ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S<T1, T2, TSerialize>>();
        var type = serializer.SerializeType(_l_serdeInfo);
        type.SerializeFieldIfNotNull<int?, Serde.NullableWrap.SerializeImpl<int, global::Serde.Int32Wrap>>(_l_serdeInfo, 0, value.FI);
        type.SerializeFieldIfNotNull<TSerialize?, Serde.NullableWrap.SerializeImpl<TSerialize, global::Serde.IdWrap<TSerialize>>>(_l_serdeInfo, 3, value.F3);
        type.End();
    }
}