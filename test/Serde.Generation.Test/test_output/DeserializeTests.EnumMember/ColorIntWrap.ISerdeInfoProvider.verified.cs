//HintName: ColorIntWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorIntWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorInt",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(ColorInt).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(ColorInt).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(ColorInt).GetField("Blue")!)
    });
}