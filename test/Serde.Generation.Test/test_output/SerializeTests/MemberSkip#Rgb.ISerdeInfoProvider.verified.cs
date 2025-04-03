//HintName: Rgb.ISerdeInfoProvider.cs

#nullable enable
partial struct Rgb
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Rgb",
    typeof(Rgb).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("red", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte, global::Serde.U8Proxy>(), typeof(Rgb).GetField("Red")),
        ("blue", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte, global::Serde.U8Proxy>(), typeof(Rgb).GetField("Blue"))
    }
    );
}