//HintName: S2.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S2
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S2",
        typeof(S2).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("E", global::Serde.SerdeInfoProvider.GetSerializeInfo<ColorEnum, ColorEnumProxy>())
        }
    );
}
