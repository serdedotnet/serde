//HintName: Rgb.ISerdeInfoProvider.g.cs

#nullable enable
partial struct Rgb
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Rgb",
    typeof(Rgb).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("red", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>(), typeof(Rgb).GetField("Red")),
        ("green", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>(), typeof(Rgb).GetField("Green")),
        ("blue", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>(), typeof(Rgb).GetField("Blue"))
    }
    );
}
