//HintName: S.IDeserializeProvider.g.cs
partial struct S : Serde.IDeserializeProvider<S>
{
    static global::Serde.IDeserialize<S> global::Serde.IDeserializeProvider<S>.Instance { get; }
        = new S._DeObj();
}
