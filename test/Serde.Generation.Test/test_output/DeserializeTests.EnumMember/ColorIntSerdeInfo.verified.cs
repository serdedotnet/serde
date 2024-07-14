//HintName: ColorIntSerdeInfo.cs
internal static class ColorIntSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorInt",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorInt).GetField("Red")!),
("green", typeof(ColorInt).GetField("Green")!),
("blue", typeof(ColorInt).GetField("Blue")!)
    });
}