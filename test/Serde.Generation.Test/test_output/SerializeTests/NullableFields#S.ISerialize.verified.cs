//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, TSerialize> : Serde.ISerialize<S<T1, T2, TSerialize>>
{
    void ISerialize<S<T1, T2, TSerialize>>.Serialize(S<T1, T2, TSerialize> value, ISerializer serializer)
    {
        var _l_typeInfo = SSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeFieldIfNotNull<int?, Serde.NullableWrap.SerializeImpl<int, Int32Wrap>>(_l_typeInfo, 0, this.FI);
        type.SerializeFieldIfNotNull<TSerialize?, Serde.NullableWrap.SerializeImpl<TSerialize, IdWrap<TSerialize>>>(_l_typeInfo, 3, this.F3);
        type.End();
    }
}