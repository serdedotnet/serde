﻿//HintName: RgbProxy.ISerialize.cs

#nullable enable

using System;
using Serde;
sealed partial class RgbProxy :Serde.ISerialize<Rgb>,Serde.ISerializeProvider<Rgb>
{
    void global::Serde.ISerialize<Rgb>.Serialize(Rgb value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            Rgb.Red => 0,
            Rgb.Green => 1,
            Rgb.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'Rgb'"),
        };
        _l_type.WriteI32(_l_info, index, (int)value);
        _l_type.End(_l_info);
    }
    static ISerialize<Rgb> ISerializeProvider<Rgb>.Instance
        => RgbProxy.Instance;

}