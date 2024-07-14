//HintName: RgbSerdeInfo.cs
internal static class RgbSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Rgb",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Rgb).GetField("Red")!),
("green", typeof(Rgb).GetField("Green")!),
("blue", typeof(Rgb).GetField("Blue")!)
    });
}