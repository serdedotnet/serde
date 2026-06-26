//HintName: NonPublic.ISerdeInfoProvider.g.cs

#nullable enable
partial record NonPublic
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "NonPublic",
        typeof(NonPublic).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("b", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
