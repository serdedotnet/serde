//HintName: ArgumentInfo.ISerdeInfoProvider.g.cs

#nullable enable
partial class ArgumentInfo
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ArgumentInfo",
    typeof(ArgumentInfo).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(ArgumentInfo).GetProperty("Name")),
        ("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(ArgumentInfo).GetProperty("Value"))
    }
    );
}
