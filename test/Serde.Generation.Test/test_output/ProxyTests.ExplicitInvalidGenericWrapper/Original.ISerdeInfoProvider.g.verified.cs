//HintName: Original.ISerdeInfoProvider.g.cs

#nullable enable
partial record struct Original
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Original",
        typeof(Original).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
        }
    );
}
