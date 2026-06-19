//HintName: Sparse.ISerdeInfoProvider.g.cs

#nullable enable
partial record Sparse
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Sparse",
    typeof(Sparse).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Sparse).GetProperty("B")),
        ("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Sparse).GetProperty("C")),
        ("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Sparse).GetProperty("A"))
    },
    new int[] { 0, 2, 5 }
    );
}
