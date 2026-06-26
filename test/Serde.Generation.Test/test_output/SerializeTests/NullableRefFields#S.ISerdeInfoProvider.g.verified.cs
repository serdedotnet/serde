//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S<T1, T2, T3, T4, T5>
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S<,,,,>).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("fS", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>()),
            new("f1", global::Serde.SerdeInfoProvider.GetSerializeInfo<T1, T1>()),
            new("f2", global::Serde.SerdeInfoProvider.GetSerializeInfo<T2, T2>()),
            new("f3", global::Serde.SerdeInfoProvider.GetSerializeInfo<T3?, Serde.NullableRefProxy.Ser<T3, T3>>()),
            new("f4", global::Serde.SerdeInfoProvider.GetSerializeInfo<T4, T4>())
        }
    );
}
