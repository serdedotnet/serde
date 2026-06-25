//HintName: Sparse.ISerdeProvider.g.cs
partial record Sparse : Serde.ISerdeProvider<Sparse, Sparse._SerdeObj, Sparse>
{
    static Sparse._SerdeObj global::Serde.ISerdeProvider<Sparse, Sparse._SerdeObj, Sparse>.Instance { get; }
        = new Sparse._SerdeObj();
}
