//HintName: Reordered.ISerdeInfoProvider.g.cs

#nullable enable
partial record Reordered
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Reordered",
        typeof(Reordered).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("B", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Reordered).GetProperty("B"),
                Ordinal = 0,
            },
            new("C", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Reordered).GetProperty("C"),
                Ordinal = 1,
            },
            new("A", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Reordered).GetProperty("A"),
                Ordinal = 2,
            }
        }
    );
}
