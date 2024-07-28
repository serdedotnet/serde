//HintName: ColorLongWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorLongWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorLong",
        typeof(ColorLong).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int64Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorLong).GetField("Red")!),
("green", typeof(ColorLong).GetField("Green")!),
("blue", typeof(ColorLong).GetField("Blue")!)
    });
}