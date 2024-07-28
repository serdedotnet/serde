//HintName: ColorULongWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorULongWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorULong",
        typeof(ColorULong).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt64Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorULong).GetField("Red")!),
("green", typeof(ColorULong).GetField("Green")!),
("blue", typeof(ColorULong).GetField("Blue")!)
    });
}