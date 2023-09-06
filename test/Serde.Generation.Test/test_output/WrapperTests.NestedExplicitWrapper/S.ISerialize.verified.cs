//HintName: S.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 1);
        type.SerializeField("sections"u8, new Serde.ImmutableArrayWrap.SerializeImpl<System.Collections.Specialized.BitVector32.Section, Outer.SectionWrap>(this.Sections));
        type.End();
    }
}