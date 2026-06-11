//HintName: C.IDeserializeProvider.g.cs
partial record C : Serde.IDeserializeProvider<C>
{
    static global::Serde.IDeserialize<C> global::Serde.IDeserializeProvider<C>.Instance { get; }
        = new C._DeObj();
}
