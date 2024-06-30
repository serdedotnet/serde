//HintName: S1.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S1 : Serde.ISerialize<S1>
{
    void ISerialize<S1>.Serialize(S1 value, ISerializer serializer)
    {
        var _l_typeInfo = S1SerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.End();
    }
}