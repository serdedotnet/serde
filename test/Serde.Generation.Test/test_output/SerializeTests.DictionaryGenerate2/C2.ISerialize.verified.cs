//HintName: C2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C2 : Serde.ISerialize<C2>
{
    void ISerialize<C2>.Serialize(C2 value, ISerializer serializer)
    {
        var _l_typeInfo = C2SerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<System.Collections.Generic.Dictionary<string, C>, Serde.DictWrap.SerializeImpl<string, StringWrap, C, IdWrap<C>>>(_l_typeInfo, 0, this.Map);
        type.End();
    }
}