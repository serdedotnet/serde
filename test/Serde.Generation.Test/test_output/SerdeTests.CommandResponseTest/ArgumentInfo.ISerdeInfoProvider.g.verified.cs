//HintName: ArgumentInfo.ISerdeInfoProvider.g.cs

#nullable enable
partial class ArgumentInfo
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ArgumentInfo",
        typeof(ArgumentInfo).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
            new("value", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
        }
    );
}
