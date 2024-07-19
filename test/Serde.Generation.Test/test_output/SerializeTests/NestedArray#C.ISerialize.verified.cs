//HintName: C.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize<C>
{
    void ISerialize<C>.Serialize(C value, ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
        var type = serializer.SerializeType(_l_serdeInfo);
        type.SerializeField<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, global::Serde.Int32Wrap>>>(_l_serdeInfo, 0, value.NestedArr);
        type.End();
    }
}