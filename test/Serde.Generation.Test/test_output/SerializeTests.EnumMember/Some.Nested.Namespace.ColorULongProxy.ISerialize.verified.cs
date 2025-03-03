﻿//HintName: Some.Nested.Namespace.ColorULongProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

sealed partial class ColorULongProxy :Serde.ISerialize<Some.Nested.Namespace.ColorULong>,Serde.ISerializeProvider<Some.Nested.Namespace.ColorULong>
{
    void global::Serde.ISerialize<Some.Nested.Namespace.ColorULong>.Serialize(Some.Nested.Namespace.ColorULong value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo<ColorULongProxy>();
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            Some.Nested.Namespace.ColorULong.Red => 0,
            Some.Nested.Namespace.ColorULong.Green => 1,
            Some.Nested.Namespace.ColorULong.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorULong'"),
        };
        _l_type.WriteU64(_l_info, index, (ulong)value);
        _l_type.End(_l_info);
    }
    static ISerialize<Some.Nested.Namespace.ColorULong> ISerializeProvider<Some.Nested.Namespace.ColorULong>.SerializeInstance
        => Some.Nested.Namespace.ColorULongProxy.Instance;

}