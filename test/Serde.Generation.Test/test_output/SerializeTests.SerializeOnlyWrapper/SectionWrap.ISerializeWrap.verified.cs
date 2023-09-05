//HintName: SectionWrap.ISerializeWrap.cs

partial record struct SectionWrap(System.Collections.Specialized.BitVector32.Section Value) : ISerializeWrap<System.Collections.Specialized.BitVector32.Section, SectionWrap>
{
    SectionWrap ISerializeWrap<System.Collections.Specialized.BitVector32.Section, SectionWrap>.Wrap(System.Collections.Specialized.BitVector32.Section value) => new(value);
}