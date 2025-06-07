//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("colorOpt", global::Serde.SerdeInfoProvider.GetSerializeInfo<Rgb?, Serde.NullableProxy.Ser<Rgb, RgbProxy>>(), typeof(C).GetField("ColorOpt"))
    }
    );
}
