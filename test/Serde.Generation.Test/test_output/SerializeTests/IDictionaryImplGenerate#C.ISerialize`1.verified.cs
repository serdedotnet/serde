//HintName: C.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize<C>
{
    void ISerialize<C>.Serialize(C value, ISerializer serializer)
    {
        var _l_typeInfo = CSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<System.Collections.Generic.IDictionary<string, int>, Serde.IDictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>>(_l_typeInfo, 0, value.RDictionary);
        type.End();
    }
}