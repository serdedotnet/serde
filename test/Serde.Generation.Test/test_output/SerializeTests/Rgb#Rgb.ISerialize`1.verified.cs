//HintName: Rgb.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct Rgb : Serde.ISerialize<Rgb>
{
    void ISerialize<Rgb>.Serialize(Rgb value, ISerializer serializer)
    {
        var _l_typeInfo = RgbSerdeTypeInfo.TypeInfo;
        var type = serializer.SerializeType(_l_typeInfo);
        type.SerializeField<byte, ByteWrap>(_l_typeInfo, 0, value.Red);
        type.SerializeField<byte, ByteWrap>(_l_typeInfo, 1, value.Green);
        type.SerializeField<byte, ByteWrap>(_l_typeInfo, 2, value.Blue);
        type.End();
    }
}