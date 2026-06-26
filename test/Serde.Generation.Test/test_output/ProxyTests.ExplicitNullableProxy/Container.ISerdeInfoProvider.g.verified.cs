//HintName: Container.ISerdeInfoProvider.g.cs

#nullable enable
partial record Container
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Container",
        typeof(Container).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("sdkDir", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Original?, Serde.NullableProxy.De<Original, Proxy>>())
            {
                MemberInfo = typeof(Container).GetProperty("SdkDir"),
            }
        }
    );
}
