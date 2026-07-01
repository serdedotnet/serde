//HintName: RgbProxy.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class RgbProxy : Serde.ISerialize<Rgb>
{
    void global::Serde.ISerialize<Rgb>.Serialize(Rgb value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_index = value switch
        {
            Rgb.Red => 0,
            Rgb.Green => 1,
            Rgb.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'Rgb'"),
        };
        serializer.WriteEnum(_l_info, _l_index);

    }

}
