//HintName: Partial.ISerdeInfoProvider.g.cs

#nullable enable
partial record Partial
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Partial",
        typeof(Partial).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("a", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
            {
                MemberInfo = typeof(Partial).GetProperty("A"),
            },
            new("b", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
