//HintName: S.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.ISerialize<S>
{
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 1);
        type.SerializeField<System.Collections.Immutable.ImmutableArray<System.Collections.Specialized.BitVector32.Section>, Serde.ImmutableArrayWrap.SerializeImpl<System.Collections.Specialized.BitVector32.Section, Outer.SectionWrap>>("sections", value.Sections);
        type.End();
    }
}