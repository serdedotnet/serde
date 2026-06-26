//HintName: Rgb.ISerdeInfoProvider.g.cs

#nullable enable
partial struct Rgb
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Rgb",
        typeof(Rgb).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("red", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>()),
            new("blue", global::Serde.SerdeInfoProvider.GetDeserializeInfo<byte, global::Serde.U8Proxy>())
        }
    );
}
