//HintName: SimpleRecord.ISerdeInfoProvider.g.cs

#nullable enable
partial record SimpleRecord
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "SimpleRecord",
        typeof(SimpleRecord).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("id", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
            new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
        }
    );
}
