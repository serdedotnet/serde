//HintName: Partial.ISerdeInfoProvider.g.cs

#nullable enable
partial record Partial
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Partial",
    typeof(Partial).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("a", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Partial).GetProperty("A")),
        ("b", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Partial).GetProperty("B"))
    }
    );
}
