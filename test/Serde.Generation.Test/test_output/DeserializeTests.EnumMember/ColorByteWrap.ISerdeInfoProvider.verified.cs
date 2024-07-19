//HintName: ColorByteWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct ColorByteWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorByte",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(ColorByte).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(ColorByte).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(ColorByte).GetField("Blue")!)
    });
}