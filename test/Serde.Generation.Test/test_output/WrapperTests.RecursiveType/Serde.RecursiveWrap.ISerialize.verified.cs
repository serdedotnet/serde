//HintName: Serde.RecursiveWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct RecursiveWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("Recursive", 1);
            type.SerializeFieldIfNotNull("next"u8, new NullableRefWrap.SerializeImpl<Recursive, RecursiveWrap>(Value.Next), Value.Next);
            type.End();
        }
    }
}