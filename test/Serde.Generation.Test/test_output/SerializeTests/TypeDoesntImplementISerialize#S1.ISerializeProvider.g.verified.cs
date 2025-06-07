//HintName: S1.ISerializeProvider.g.cs
partial struct S1 : Serde.ISerializeProvider<S1>
{
    static global::Serde.ISerialize<S1> global::Serde.ISerializeProvider<S1>.Instance { get; }
        = new S1._SerObj();
}
