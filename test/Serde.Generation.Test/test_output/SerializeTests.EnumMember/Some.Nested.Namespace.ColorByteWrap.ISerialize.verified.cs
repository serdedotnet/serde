//HintName: Some.Nested.Namespace.ColorByteWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial struct ColorByteWrap : Serde.ISerialize<Some.Nested.Namespace.ColorByte>
    {
        void ISerialize<Some.Nested.Namespace.ColorByte>.Serialize(Some.Nested.Namespace.ColorByte value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>();
            var index = value switch
            {
                Some.Nested.Namespace.ColorByte.Red => 0,
                Some.Nested.Namespace.ColorByte.Green => 1,
                Some.Nested.Namespace.ColorByte.Blue => 2,
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorByte'"),
            };
            serializer.SerializeEnumValue(_l_serdeInfo, index, (byte)value, default(global::Serde.ByteWrap));
        }
    }
}