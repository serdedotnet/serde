//HintName: Some.Nested.Namespace.ColorIntWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial struct ColorIntWrap : Serde.ISerialize<Some.Nested.Namespace.ColorInt>
    {
        void ISerialize<Some.Nested.Namespace.ColorInt>.Serialize(Some.Nested.Namespace.ColorInt value, ISerializer serializer)
        {
            var _l_typeInfo = Some.Nested.Namespace.ColorIntSerdeTypeInfo.TypeInfo;
            var index = value switch
            {
                Some.Nested.Namespace.ColorInt.Red => 0,
                Some.Nested.Namespace.ColorInt.Green => 1,
                Some.Nested.Namespace.ColorInt.Blue => 2,
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorInt'"),
            };
            serializer.SerializeEnumValue(_l_typeInfo, index, (int)value, default(Int32Wrap));
        }
    }
}