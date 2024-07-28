//HintName: ColorEnumWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorEnumWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorEnum",
        typeof(ColorEnum).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorEnum).GetField("Red")!),
("green", typeof(ColorEnum).GetField("Green")!),
("blue", typeof(ColorEnum).GetField("Blue")!)
    });
}