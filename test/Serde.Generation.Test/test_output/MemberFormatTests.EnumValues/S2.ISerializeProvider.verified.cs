//HintName: S2.ISerializeProvider.cs
partial struct S2 : Serde.ISerializeProvider<S2>
{
    static global::Serde.ISerialize<S2> global::Serde.ISerializeProvider<S2>.Instance { get; }
        = new S2._SerObj();
}
