//HintName: RgbWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct RgbWrap : Serde.ISerialize<Rgb>
{
    void ISerialize<Rgb>.Serialize(Rgb value, ISerializer serializer)
    {
        var _l_typeInfo = RgbSerdeTypeInfo.TypeInfo;
        var index = value switch
        {
            Rgb.Red => 0,
            Rgb.Green => 1,
            Rgb.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'Rgb'"),
        };
        serializer.SerializeEnumValue(_l_typeInfo, index, (int)value, default(Int32Wrap));
    }
}