//HintName: ColorByteWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorByteWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorByte",
        typeof(ColorByte).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorByte).GetField("Red")!),
("green", typeof(ColorByte).GetField("Green")!),
("blue", typeof(ColorByte).GetField("Blue")!)
    });
}