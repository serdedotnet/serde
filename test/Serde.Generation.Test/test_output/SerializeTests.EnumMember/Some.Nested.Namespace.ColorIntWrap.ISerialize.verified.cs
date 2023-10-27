//HintName: Some.Nested.Namespace.ColorIntWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial record struct ColorIntWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                Some.Nested.Namespace.ColorInt.Red => "red",
                Some.Nested.Namespace.ColorInt.Green => "green",
                Some.Nested.Namespace.ColorInt.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorInt", name, new Int32Wrap((int)Value));
        }
    }
}