//HintName: ColorULongSerdeInfo.cs
internal static class ColorULongSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorULong",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorULong).GetField("Red")!),
("green", typeof(ColorULong).GetField("Green")!),
("blue", typeof(ColorULong).GetField("Blue")!)
    });
}