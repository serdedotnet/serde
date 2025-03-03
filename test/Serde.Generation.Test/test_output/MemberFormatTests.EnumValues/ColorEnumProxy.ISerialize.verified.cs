//HintName: ColorEnumProxy.ISerialize.cs

#nullable enable

using System;
using Serde;
sealed partial class ColorEnumProxy :Serde.ISerialize<ColorEnum>,Serde.ISerializeProvider<ColorEnum>
{
    void global::Serde.ISerialize<ColorEnum>.Serialize(ColorEnum value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumProxy>();
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            ColorEnum.Red => 0,
            ColorEnum.Green => 1,
            ColorEnum.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
        };
        _l_type.WriteI32(_l_info, index, (int)value);
        _l_type.End(_l_info);
    }
    static ISerialize<ColorEnum> ISerializeProvider<ColorEnum>.SerializeInstance
        => ColorEnumProxy.Instance;

}