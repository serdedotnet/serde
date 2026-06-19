//HintName: NonPublic.ISerdeInfoProvider.g.cs

#nullable enable
partial record NonPublic
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "NonPublic",
    typeof(NonPublic).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("b", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(NonPublic).GetProperty("B"))
    }
    );
}
