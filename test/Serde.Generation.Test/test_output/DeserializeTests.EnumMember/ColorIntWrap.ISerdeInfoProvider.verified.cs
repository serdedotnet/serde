//HintName: ColorIntWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorIntWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorInt",
        typeof(ColorInt).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorInt).GetField("Red")!),
("green", typeof(ColorInt).GetField("Green")!),
("blue", typeof(ColorInt).GetField("Blue")!)
    });
}