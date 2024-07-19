//HintName: S2.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S2 : Serde.ISerialize<S2>
{
    void ISerialize<S2>.Serialize(S2 value, ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S2>();
        var type = serializer.SerializeType(_l_serdeInfo);
        type.SerializeField<ColorEnum, ColorEnumWrap>(_l_serdeInfo, 0, value.E);
        type.End();
    }
}