﻿//HintName: Outer.SectionWrap.ISerializeWrap.cs

partial class Outer
{
    partial record struct SectionWrap(System.Collections.Specialized.BitVector32.Section Value) : Serde.ISerializeWrap<System.Collections.Specialized.BitVector32.Section, SectionWrap>
    {
        SectionWrap Serde.ISerializeWrap<System.Collections.Specialized.BitVector32.Section, SectionWrap>.Wrap(System.Collections.Specialized.BitVector32.Section value) => new(value);
    }
}