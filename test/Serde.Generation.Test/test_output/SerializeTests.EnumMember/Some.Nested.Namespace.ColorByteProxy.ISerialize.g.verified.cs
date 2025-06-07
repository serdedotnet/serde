//HintName: Some.Nested.Namespace.ColorByteProxy.ISerialize.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial class ColorByteProxy : Serde.ISerialize<Some.Nested.Namespace.ColorByte>
{
    void global::Serde.ISerialize<Some.Nested.Namespace.ColorByte>.Serialize(Some.Nested.Namespace.ColorByte value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            Some.Nested.Namespace.ColorByte.Red => 0,
            Some.Nested.Namespace.ColorByte.Green => 1,
            Some.Nested.Namespace.ColorByte.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorByte'"),
        };
        _l_type.WriteU8(_l_info, index, (byte)value);
        _l_type.End(_l_info);
    }

}
