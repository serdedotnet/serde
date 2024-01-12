//HintName: SectionWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial record struct SectionWrap : Serde.ISerialize<System.Collections.Specialized.BitVector32.Section>
{
    void ISerialize<System.Collections.Specialized.BitVector32.Section>.Serialize(System.Collections.Specialized.BitVector32.Section value, ISerializer serializer)
    {
        var type = serializer.SerializeType("Section", 2);
        type.SerializeField<short, Int16Wrap>("mask", value.Mask);
        type.SerializeField<short, Int16Wrap>("offset", value.Offset);
        type.End();
    }
}