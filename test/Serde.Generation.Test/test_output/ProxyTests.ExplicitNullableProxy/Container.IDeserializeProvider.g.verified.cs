//HintName: Container.IDeserializeProvider.g.cs
partial record Container : Serde.IDeserializeProvider<Container>
{
    static global::Serde.IDeserialize<Container> global::Serde.IDeserializeProvider<Container>.Instance { get; }
        = new Container._DeObj();
}
