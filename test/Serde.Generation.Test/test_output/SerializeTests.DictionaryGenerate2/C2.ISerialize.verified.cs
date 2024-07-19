//HintName: C2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C2 : Serde.ISerialize<C2>
{
    void ISerialize<C2>.Serialize(C2 value, ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C2>();
        var type = serializer.SerializeType(_l_serdeInfo);
        type.SerializeField<System.Collections.Generic.Dictionary<string, C>, Serde.DictWrap.SerializeImpl<string, global::Serde.StringWrap, C, global::Serde.IdWrap<C>>>(_l_serdeInfo, 0, value.Map);
        type.End();
    }
}