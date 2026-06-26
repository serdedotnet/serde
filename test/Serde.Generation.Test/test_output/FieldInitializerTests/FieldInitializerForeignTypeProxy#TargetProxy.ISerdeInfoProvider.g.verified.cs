//HintName: TargetProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial struct TargetProxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Target",
        typeof(Target).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("x", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
            new("y", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
