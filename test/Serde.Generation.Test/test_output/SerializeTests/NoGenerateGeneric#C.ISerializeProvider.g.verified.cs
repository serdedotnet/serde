//HintName: C.ISerializeProvider.g.cs
partial class C<T> : Serde.ISerializeProvider<C<T>>
{
    static global::Serde.ISerialize<C<T>> global::Serde.ISerializeProvider<C<T>>.Instance { get; }
        = new C<T>._SerObj();
}
