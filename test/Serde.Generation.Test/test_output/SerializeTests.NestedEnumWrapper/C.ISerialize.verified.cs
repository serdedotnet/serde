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
        type.SerializeFieldIfNotNull<Rgb?, Serde.NullableWrap.SerializeImpl<Rgb, RgbWrap>>(_l_serdeInfo, 0, value.ColorOpt);
        type.End();
    }
}