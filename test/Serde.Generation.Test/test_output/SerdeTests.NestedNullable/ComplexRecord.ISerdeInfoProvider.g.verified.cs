//HintName: ComplexRecord.ISerdeInfoProvider.g.cs

#nullable enable
partial record ComplexRecord
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ComplexRecord",
        typeof(ComplexRecord).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("id", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
            new("description", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>()),
            new("nestedRecord", global::Serde.SerdeInfoProvider.GetSerializeInfo<SimpleRecord?, Serde.NullableRefProxy.Ser<SimpleRecord, SimpleRecord>>())
        }
    );
}
