//HintName: Address.ISerdeInfoProvider.g.cs

#nullable enable
partial class Address
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Address",
        typeof(Address).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("name", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            {
                MemberInfo = typeof(Address).GetField("Name"),
            },
            new("line1", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
            new("city", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            {
                MemberInfo = typeof(Address).GetField("City"),
            },
            new("state", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
            new("zip", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
        }
    );
}
