//HintName: SetToNull.ISerdeInfoProvider.g.cs

#nullable enable
partial record struct SetToNull
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "SetToNull",
    typeof(SetToNull).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("present", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(SetToNull).GetProperty("Present")),
        ("missing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(SetToNull).GetProperty("Missing")),
        ("throwMissing", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(SetToNull).GetProperty("ThrowMissing"))
    }
    );
}
