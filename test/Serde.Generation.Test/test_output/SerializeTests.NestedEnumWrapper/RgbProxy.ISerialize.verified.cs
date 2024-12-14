//HintName: RgbProxy.ISerialize.cs

#nullable enable
using System;
using Serde;

sealed partial class RgbProxy : Serde.ISerialize<Rgb>, Serde.ISerializeProvider<Rgb>
{
    void ISerialize<Rgb>.Serialize(Rgb value, ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<RgbProxy>();
        var index = value switch
        {
            Rgb.Red => 0,
            Rgb.Green => 1,
            Rgb.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'Rgb'"),
        };
        serializer.SerializeEnumValue(_l_serdeInfo, index, (int)value, global::Serde.Int32Proxy.Instance);
    }

    static ISerialize<Rgb> ISerializeProvider<Rgb>.SerializeInstance => RgbProxy.Instance;
}