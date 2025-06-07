//HintName: Original.IDeserializeProvider.g.cs
partial record struct Original : Serde.IDeserializeProvider<Original>
{
    static global::Serde.IDeserialize<Original> global::Serde.IDeserializeProvider<Original>.Instance { get; }
        = new Original._DeObj();
}
