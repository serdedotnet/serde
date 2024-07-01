//HintName: ColorEnumWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct ColorEnumWrap : Serde.ISerialize<ColorEnum>
{
    void ISerialize<ColorEnum>.Serialize(ColorEnum value, ISerializer serializer)
    {
        var _l_typeInfo = ColorEnumSerdeTypeInfo.TypeInfo;
        var index = value switch
        {
            ColorEnum.Red => 0,
            ColorEnum.Green => 1,
            ColorEnum.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
        };
        serializer.SerializeEnumValue(_l_typeInfo, index, (int)value, default(Int32Wrap));
    }
}