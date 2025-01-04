//HintName: Some.Nested.Namespace.ColorByteProxy.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Some.Nested.Namespace;

sealed partial class ColorByteProxy :Serde.ISerialize<Some.Nested.Namespace.ColorByte>,Serde.ISerializeProvider<Some.Nested.Namespace.ColorByte>
{
    void global::Serde.ISerialize<Some.Nested.Namespace.ColorByte>.Serialize(Some.Nested.Namespace.ColorByte value, global::Serde.ISerializer serializer)
    {
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorByteProxy>();
        var index = value switch
        {
            Some.Nested.Namespace.ColorByte.Red => 0,
            Some.Nested.Namespace.ColorByte.Green => 1,
            Some.Nested.Namespace.ColorByte.Blue => 2,
            var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorByte'"),
        };
        serializer.SerializeEnumValue(_l_serdeInfo, index, (byte)value, global::Serde.ByteProxy.Instance);

    }
    static ISerialize<Some.Nested.Namespace.ColorByte> ISerializeProvider<Some.Nested.Namespace.ColorByte>.SerializeInstance
        => Some.Nested.Namespace.ColorByteProxy.Instance;

}