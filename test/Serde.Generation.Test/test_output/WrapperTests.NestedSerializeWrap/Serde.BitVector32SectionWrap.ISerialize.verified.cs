//HintName: Serde.BitVector32SectionWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("Section", 2);
            type.SerializeField("mask"u8, new Int16Wrap(Value.Mask));
            type.SerializeField("offset"u8, new Int16Wrap(Value.Offset));
            type.End();
        }
    }
}