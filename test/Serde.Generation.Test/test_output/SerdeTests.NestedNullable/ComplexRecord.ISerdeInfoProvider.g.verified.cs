//HintName: ComplexRecord.ISerdeInfoProvider.g.cs

#nullable enable
partial record ComplexRecord
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ComplexRecord",
    typeof(ComplexRecord).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("id", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(ComplexRecord).GetProperty("Id")),
        ("description", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(), typeof(ComplexRecord).GetProperty("Description")),
        ("nestedRecord", global::Serde.SerdeInfoProvider.GetSerializeInfo<SimpleRecord?, Serde.NullableRefProxy.Ser<SimpleRecord, SimpleRecord>>(), typeof(ComplexRecord).GetProperty("NestedRecord"))
    }
    );
}
