//HintName: S.ISerializeProvider.g.cs
partial struct S<T1, T2, TSerialize> : Serde.ISerializeProvider<S<T1, T2, TSerialize>>
{
    static global::Serde.ISerialize<S<T1, T2, TSerialize>> global::Serde.ISerializeProvider<S<T1, T2, TSerialize>>.Instance { get; }
        = new S<T1, T2, TSerialize>._SerObj();
}
