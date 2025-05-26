//HintName: S.ISerdeProvider.cs
partial struct S : Serde.ISerdeProvider<S, S._SerdeObj, S>
{
    static S._SerdeObj global::Serde.ISerdeProvider<S, S._SerdeObj, S>.Instance { get; }
        = new S._SerdeObj();
}
