//HintName: S.ISerializeProvider.cs
partial struct S : Serde.ISerializeProvider<S>
{
    static global::Serde.ISerialize<S> global::Serde.ISerializeProvider<S>.Instance { get; }
        = new S._SerObj();
}
