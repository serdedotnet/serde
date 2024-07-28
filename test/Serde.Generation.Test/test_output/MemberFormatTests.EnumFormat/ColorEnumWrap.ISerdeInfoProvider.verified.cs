//HintName: ColorEnumWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorEnumWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorEnum",
        typeof(ColorEnum).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("Red", typeof(ColorEnum).GetField("Red")!),
("Green", typeof(ColorEnum).GetField("Green")!),
("Blue", typeof(ColorEnum).GetField("Blue")!)
    });
}