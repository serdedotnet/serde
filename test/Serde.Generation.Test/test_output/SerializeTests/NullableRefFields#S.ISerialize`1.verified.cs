//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S<T1, T2, T3, T4, T5> : Serde.ISerialize<S<T1, T2, T3, T4, T5>>
{
    void ISerialize<S<T1, T2, T3, T4, T5>>.Serialize(S<T1, T2, T3, T4, T5> value, ISerializer serializer)
    {
        var _l_typeInfo = SSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>(_l_typeInfo, 0, value.FS);
        type.SerializeField<T1, IdWrap<T1>>(_l_typeInfo, 1, value.F1);
        type.SerializeFieldIfNotNull<T2, IdWrap<T2>>(_l_typeInfo, 2, value.F2);
        type.SerializeFieldIfNotNull<T3?, Serde.NullableRefWrap.SerializeImpl<T3, IdWrap<T3>>>(_l_typeInfo, 3, value.F3);
        type.SerializeFieldIfNotNull<T4, IdWrap<T4>>(_l_typeInfo, 4, value.F4);
        type.End();
    }
}