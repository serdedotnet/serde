//HintName: C.ISerializeProvider.cs
partial class C : Serde.ISerializeProvider<C>
{
    static global::Serde.ISerialize<C> global::Serde.ISerializeProvider<C>.Instance { get; }
        = new C._SerObj();
}
