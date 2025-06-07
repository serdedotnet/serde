//HintName: S.ISerializeProvider.g.cs
partial struct S<T1, T2, T3, T4, T5> : Serde.ISerializeProvider<S<T1, T2, T3, T4, T5>>
{
    static global::Serde.ISerialize<S<T1, T2, T3, T4, T5>> global::Serde.ISerializeProvider<S<T1, T2, T3, T4, T5>>.Instance { get; }
        = new S<T1, T2, T3, T4, T5>._SerObj();
}
