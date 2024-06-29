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
            var name = value switch
            {
                Some.Nested.Namespace.ColorInt.Red => "red",
                Some.Nested.Namespace.ColorInt.Green => "green",
                Some.Nested.Namespace.ColorInt.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorInt", name, (int)value, default(Int32Wrap));
        }
    }
}