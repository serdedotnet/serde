//HintName: Some.Nested.Namespace.ColorIntWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial record struct ColorIntWrap : Serde.ISerialize<Some.Nested.Namespace.ColorInt>
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
            serializer.SerializeEnumValue("ColorInt", name, new Int32Wrap((int)value));
        }
    }
}