//HintName: B.ISerializeProvider.cs
partial record B : Serde.ISerializeProvider<B>
{
    static global::Serde.ISerialize<B> global::Serde.ISerializeProvider<B>.Instance { get; }
        = new B._SerObj();
}
