//HintName: R.ISerdeInfoProvider.g.cs

#nullable enable
partial record R
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "R",
        typeof(R).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("a", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
            new("b", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
        }
    );
}
