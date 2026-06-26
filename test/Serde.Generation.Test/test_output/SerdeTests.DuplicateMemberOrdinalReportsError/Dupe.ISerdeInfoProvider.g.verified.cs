//HintName: Dupe.ISerdeInfoProvider.g.cs

#nullable enable
partial record Dupe
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Dupe",
        typeof(Dupe).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("a", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Dupe).GetProperty("A"),
                Ordinal = 0,
            },
            new("b", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Dupe).GetProperty("B"),
                Ordinal = 0,
            }
        }
    );
}
