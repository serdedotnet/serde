//HintName: Some.Nested.Namespace.ColorLongWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial record struct ColorLongWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                Some.Nested.Namespace.ColorLong.Red => "red",
                Some.Nested.Namespace.ColorLong.Green => "green",
                Some.Nested.Namespace.ColorLong.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorLong", name, new Int64Wrap((long)Value));
        }
    }
}