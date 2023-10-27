//HintName: SectionWrap.ISerializeWrap.cs

partial record struct SectionWrap : Serde.ISerializeWrap<System.Collections.Specialized.BitVector32.Section, SectionWrap>
{
    static SectionWrap Serde.ISerializeWrap<System.Collections.Specialized.BitVector32.Section, SectionWrap>.Create(System.Collections.Specialized.BitVector32.Section value) => new(value);
}