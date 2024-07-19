//HintName: RgbWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct RgbWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Rgb",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<RgbWrap>(), typeof(Rgb).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<RgbWrap>(), typeof(Rgb).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<RgbWrap>(), typeof(Rgb).GetField("Blue")!)
    });
}