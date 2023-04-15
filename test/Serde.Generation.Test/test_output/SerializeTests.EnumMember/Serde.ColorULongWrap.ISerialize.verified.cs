//HintName: Serde.ColorULongWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct ColorULongWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                Some.Nested.Namespace.ColorULong.Red => "red",
                Some.Nested.Namespace.ColorULong.Green => "green",
                Some.Nested.Namespace.ColorULong.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorULong", name, new UInt64Wrap((ulong)Value));
        }
    }
}