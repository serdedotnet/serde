//HintName: B.IDeserializeProvider.cs
partial class B : Serde.IDeserializeProvider<B>
{
    static global::Serde.IDeserialize<B> global::Serde.IDeserializeProvider<B>.Instance { get; }
        = new B._DeObj();
}
