﻿//HintName: Some.Nested.Namespace.ColorLongProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

sealed partial class ColorLongProxy :Serde.ISerialize<Some.Nested.Namespace.ColorLong>,Serde.ISerializeProvider<Some.Nested.Namespace.ColorLong>
{
    void global::Serde.ISerialize<Some.Nested.Namespace.ColorLong>.Serialize(Some.Nested.Namespace.ColorLong value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            Some.Nested.Namespace.ColorLong.Red => 0,
            Some.Nested.Namespace.ColorLong.Green => 1,
            Some.Nested.Namespace.ColorLong.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorLong'"),
        };
        _l_type.WriteI64(_l_info, index, (long)value);
        _l_type.End(_l_info);
    }
    static ISerialize<Some.Nested.Namespace.ColorLong> ISerializeProvider<Some.Nested.Namespace.ColorLong>.Instance
        => Some.Nested.Namespace.ColorLongProxy.Instance;

}