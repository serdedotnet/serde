//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S<T1, T2, T3, T4, T5>
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
    typeof(S<,,,,>).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("fS", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(), typeof(S<,,,,>).GetField("FS")),
        ("f1", global::Serde.SerdeInfoProvider.GetSerializeInfo<T1, T1>(), typeof(S<,,,,>).GetField("F1")),
        ("f2", global::Serde.SerdeInfoProvider.GetSerializeInfo<T2, T2>(), typeof(S<,,,,>).GetField("F2")),
        ("f3", global::Serde.SerdeInfoProvider.GetSerializeInfo<T3?, Serde.NullableRefProxy.Ser<T3, T3>>(), typeof(S<,,,,>).GetField("F3")),
        ("f4", global::Serde.SerdeInfoProvider.GetSerializeInfo<T4, T4>(), typeof(S<,,,,>).GetField("F4"))
    }
    );
}
