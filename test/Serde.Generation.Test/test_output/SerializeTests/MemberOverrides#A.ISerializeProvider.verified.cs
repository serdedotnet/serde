//HintName: A.ISerializeProvider.cs
partial record A : Serde.ISerializeProvider<A>
{
    static global::Serde.ISerialize<A> global::Serde.ISerializeProvider<A>.Instance { get; }
        = new A._SerObj();
}
