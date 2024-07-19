//HintName: Rgb.ISerdeInfoProvider.cs

#nullable enable
partial struct Rgb : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Rgb",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Rgb).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Rgb).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Rgb).GetField("Blue")!)
    });
}