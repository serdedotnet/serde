//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("p", global::Serde.SerdeInfoProvider.GetSerializeInfo<Point, PointProxy>()),
            new("optP", global::Serde.SerdeInfoProvider.GetSerializeInfo<Point?, Serde.NullableRefProxy.Ser<Point, PointProxy>>())
        }
    );
}
