//HintName: PointWrap.ISerdeInfoProvider.g.cs

#nullable enable
partial struct PointWrap
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Point",
        typeof(Point).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
            new("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
