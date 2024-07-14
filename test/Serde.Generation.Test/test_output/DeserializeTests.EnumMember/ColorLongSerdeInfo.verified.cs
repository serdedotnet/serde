//HintName: ColorLongSerdeInfo.cs
internal static class ColorLongSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorLong",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorLong).GetField("Red")!),
("green", typeof(ColorLong).GetField("Green")!),
("blue", typeof(ColorLong).GetField("Blue")!)
    });
}