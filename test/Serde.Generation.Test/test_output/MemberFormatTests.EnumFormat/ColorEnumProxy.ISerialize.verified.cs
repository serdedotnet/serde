//HintName: ColorEnumProxy.ISerialize.cs

#nullable enable

using System;
using Serde;
sealed partial class ColorEnumProxy :Serde.ISerialize<ColorEnum>,Serde.ISerializeProvider<ColorEnum>
{
    void global::Serde.ISerialize<ColorEnum>.Serialize(ColorEnum value, global::Serde.ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumProxy>();
        var index = value switch
        {
            ColorEnum.Red => 0,
            ColorEnum.Green => 1,
            ColorEnum.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
        };
        serializer.SerializeEnumValue(_l_serdeInfo, index, (int)value, global::Serde.Int32Proxy.Instance);

    }
    static ISerialize<ColorEnum> ISerializeProvider<ColorEnum>.SerializeInstance
        => ColorEnumProxy.Instance;

}