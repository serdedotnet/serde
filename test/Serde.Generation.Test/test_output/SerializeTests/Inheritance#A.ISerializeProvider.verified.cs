//HintName: A.ISerializeProvider.cs
partial class A : Serde.ISerializeProvider<A>
{
    static global::Serde.ISerialize<A> global::Serde.ISerializeProvider<A>.Instance { get; }
        = new A._SerObj();
}
