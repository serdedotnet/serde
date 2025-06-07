//HintName: Some.Nested.Namespace.ColorIntProxy.ISerialize.g.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

partial class ColorIntProxy : Serde.ISerialize<Some.Nested.Namespace.ColorInt>
{
    void global::Serde.ISerialize<Some.Nested.Namespace.ColorInt>.Serialize(Some.Nested.Namespace.ColorInt value, global::Serde.ISerializer serializer)
    {
        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
        var _l_type = serializer.WriteType(_l_info);
        var index = value switch
        {
            Some.Nested.Namespace.ColorInt.Red => 0,
            Some.Nested.Namespace.ColorInt.Green => 1,
            Some.Nested.Namespace.ColorInt.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorInt'"),
        };
        _l_type.WriteI32(_l_info, index, (int)value);
        _l_type.End(_l_info);
    }

}
