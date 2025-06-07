//HintName: SectionWrap.ISerializeProvider.g.cs
partial record struct SectionWrap : Serde.ISerializeProvider<System.Collections.Specialized.BitVector32.Section>
{
    static global::Serde.ISerialize<System.Collections.Specialized.BitVector32.Section> global::Serde.ISerializeProvider<System.Collections.Specialized.BitVector32.Section>.Instance { get; }
        = new SectionWrap._SerObj();
}
