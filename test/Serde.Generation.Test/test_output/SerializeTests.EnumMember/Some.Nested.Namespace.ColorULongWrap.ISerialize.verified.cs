//HintName: Some.Nested.Namespace.ColorULongWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial struct ColorULongWrap : Serde.ISerialize<Some.Nested.Namespace.ColorULong>
    {
        void ISerialize<Some.Nested.Namespace.ColorULong>.Serialize(Some.Nested.Namespace.ColorULong value, ISerializer serializer)
        {
            var name = value switch
            {
                Some.Nested.Namespace.ColorULong.Red => "red",
                Some.Nested.Namespace.ColorULong.Green => "green",
                Some.Nested.Namespace.ColorULong.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorULong", name, (ulong)value, default(UInt64Wrap));
        }
    }
}