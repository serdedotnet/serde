//HintName: B.IDeserializeProvider.g.cs
partial record B : Serde.IDeserializeProvider<B>
{
    static global::Serde.IDeserialize<B> global::Serde.IDeserializeProvider<B>.Instance { get; }
        = new B._DeObj();
}
