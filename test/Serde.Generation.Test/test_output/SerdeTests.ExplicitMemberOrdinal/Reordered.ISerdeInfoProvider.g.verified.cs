//HintName: Reordered.ISerdeInfoProvider.g.cs

#nullable enable
partial record Reordered
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Reordered",
    typeof(Reordered).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Reordered).GetProperty("B")),
        ("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Reordered).GetProperty("C")),
        ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Reordered).GetProperty("A"))
    },
    new int[] { 0, 1, 2 }
    );
}
