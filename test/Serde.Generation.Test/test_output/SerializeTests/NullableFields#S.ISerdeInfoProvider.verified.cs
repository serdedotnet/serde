//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S<T1, T2, TSerialize> : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S<,,>).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("fI", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableProxy.Serialize<int, global::Serde.I32Proxy>>(), typeof(S<,,>).GetField("FI")),
            ("f3", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableProxy.Serialize<TSerialize, TSerialize>>(), typeof(S<,,>).GetField("F3"))
        }
    );
}