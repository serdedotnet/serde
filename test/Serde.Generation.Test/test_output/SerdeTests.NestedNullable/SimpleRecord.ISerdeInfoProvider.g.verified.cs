//HintName: SimpleRecord.ISerdeInfoProvider.g.cs

#nullable enable
partial record SimpleRecord
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "SimpleRecord",
    typeof(SimpleRecord).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("id", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(SimpleRecord).GetProperty("Id")),
        ("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(SimpleRecord).GetProperty("Name"))
    }
    );
}
