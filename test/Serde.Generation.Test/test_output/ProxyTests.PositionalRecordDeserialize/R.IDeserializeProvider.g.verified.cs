//HintName: R.IDeserializeProvider.g.cs
partial record R : Serde.IDeserializeProvider<R>
{
    static global::Serde.IDeserialize<R> global::Serde.IDeserializeProvider<R>.Instance { get; }
        = new R._DeObj();
}
