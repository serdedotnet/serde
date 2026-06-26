//HintName: B.ISerdeInfoProvider.g.cs

#nullable enable
partial record B
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "B",
        typeof(B).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("y", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
            new("x", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
