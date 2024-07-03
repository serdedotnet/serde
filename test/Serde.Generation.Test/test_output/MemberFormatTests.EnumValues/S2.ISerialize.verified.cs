//HintName: S2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S2 : Serde.ISerialize<S2>
{
    void ISerialize<S2>.Serialize(S2 value, ISerializer serializer)
    {
        var _l_typeInfo = S2SerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<ColorEnum, ColorEnumWrap>(_l_typeInfo, 0, value.E);
        type.End();
    }
}