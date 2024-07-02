//HintName: Some.Nested.Namespace.ColorLongWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial struct ColorLongWrap : Serde.ISerialize<Some.Nested.Namespace.ColorLong>
    {
        void ISerialize<Some.Nested.Namespace.ColorLong>.Serialize(Some.Nested.Namespace.ColorLong value, ISerializer serializer)
        {
            var _l_typeInfo = Some.Nested.Namespace.ColorLongSerdeTypeInfo.TypeInfo;
            var index = value switch
            {
                Some.Nested.Namespace.ColorLong.Red => 0,
                Some.Nested.Namespace.ColorLong.Green => 1,
                Some.Nested.Namespace.ColorLong.Blue => 2,
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorLong'"),
            };
            serializer.SerializeEnumValue(_l_typeInfo, index, (long)value, default(Int64Wrap));
        }
    }
}