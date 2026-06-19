//HintName: Dupe.ISerdeInfoProvider.g.cs

#nullable enable
partial record Dupe
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Dupe",
    typeof(Dupe).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("a", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Dupe).GetProperty("A")),
        ("b", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Dupe).GetProperty("B"))
    },
    new int[] { 0, 0 }
    );
}
