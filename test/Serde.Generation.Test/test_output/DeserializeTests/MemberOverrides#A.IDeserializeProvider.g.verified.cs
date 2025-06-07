//HintName: A.IDeserializeProvider.g.cs
partial record A : Serde.IDeserializeProvider<A>
{
    static global::Serde.IDeserialize<A> global::Serde.IDeserializeProvider<A>.Instance { get; }
        = new A._DeObj();
}
