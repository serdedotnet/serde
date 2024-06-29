//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize<S>
{
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        var _l_typeInfo = SSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, this.One);
        type.SerializeField<int, Int32Wrap>(_l_typeInfo, 1, this.TwoWord);
        type.End();
    }
}