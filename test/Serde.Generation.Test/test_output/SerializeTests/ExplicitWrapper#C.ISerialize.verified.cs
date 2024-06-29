//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize<C>
{
    void ISerialize<C>.Serialize(C value, ISerializer serializer)
    {
        var _l_typeInfo = CSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<S, SWrap>(_l_typeInfo, 0, this.S);
        type.End();
    }
}