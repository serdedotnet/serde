//HintName: Sparse.ISerdeInfoProvider.g.cs

#nullable enable
partial record Sparse
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Sparse",
        typeof(Sparse).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Sparse).GetProperty("B"),
                Ordinal = 0,
            },
            new("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Sparse).GetProperty("C"),
                Ordinal = 2,
            },
            new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Sparse).GetProperty("A"),
                Ordinal = 5,
            }
        }
    );
}
