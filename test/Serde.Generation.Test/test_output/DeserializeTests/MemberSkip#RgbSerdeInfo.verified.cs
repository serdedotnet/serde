//HintName: RgbSerdeInfo.cs
internal static class RgbSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Rgb",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Rgb).GetField("Red")!),
("blue", typeof(Rgb).GetField("Blue")!)
    });
}