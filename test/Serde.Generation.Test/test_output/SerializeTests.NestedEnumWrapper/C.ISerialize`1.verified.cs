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
        type.SerializeFieldIfNotNull<Rgb?, Serde.NullableWrap.SerializeImpl<Rgb, global::RgbWrap>>(_l_typeInfo, 0, value.ColorOpt);
        type.End();
    }
}