//HintName: ColorEnumWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorEnumWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorEnum",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>(), typeof(ColorEnum).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>(), typeof(ColorEnum).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>(), typeof(ColorEnum).GetField("Blue")!)
    });
}