//HintName: Container.ISerdeInfoProvider.g.cs

#nullable enable
partial record Container
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Container",
    typeof(Container).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("sdkDir", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Original?, Serde.NullableProxy.De<Original, Proxy>>(), typeof(Container).GetProperty("SdkDir"))
    }
    );
}
