﻿//HintName: Some.Nested.Namespace.ColorByteWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

namespace Some.Nested.Namespace
{
    partial record struct ColorByteWrap : Serde.ISerialize<Some.Nested.Namespace.ColorByte>
    {
        void ISerialize<Some.Nested.Namespace.ColorByte>.Serialize(Some.Nested.Namespace.ColorByte value, ISerializer serializer)
        {
            var name = value switch
            {
                Some.Nested.Namespace.ColorByte.Red => "red",
                Some.Nested.Namespace.ColorByte.Green => "green",
                Some.Nested.Namespace.ColorByte.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorByte", name, new ByteWrap((byte)value));
        }
    }
}