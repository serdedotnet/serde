//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S<T1, T2, TSerialize>
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S<,,>).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("fI", global::Serde.SerdeInfoProvider.GetSerializeInfo<int?, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>()),
            new("f3", global::Serde.SerdeInfoProvider.GetSerializeInfo<TSerialize?, Serde.NullableProxy.Ser<TSerialize, TSerialize>>())
        }
    );
}
