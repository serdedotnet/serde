//HintName: Some.Nested.Namespace.ColorLongProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

sealed partial class ColorLongProxy :Serde.ISerialize<Some.Nested.Namespace.ColorLong>,Serde.ISerializeProvider<Some.Nested.Namespace.ColorLong>
{
    void global::Serde.ISerialize<Some.Nested.Namespace.ColorLong>.Serialize(Some.Nested.Namespace.ColorLong value, global::Serde.ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorLongProxy>();
        var index = value switch
        {
            Some.Nested.Namespace.ColorLong.Red => 0,
            Some.Nested.Namespace.ColorLong.Green => 1,
            Some.Nested.Namespace.ColorLong.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorLong'"),
        };
        serializer.SerializeEnumValue(_l_serdeInfo, index, (long)value, global::Serde.Int64Proxy.Instance);

    }
    static ISerialize<Some.Nested.Namespace.ColorLong> ISerializeProvider<Some.Nested.Namespace.ColorLong>.SerializeInstance
        => Some.Nested.Namespace.ColorLongProxy.Instance;

}