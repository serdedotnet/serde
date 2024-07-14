//HintName: ColorEnumSerdeInfo.cs
internal static class ColorEnumSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorEnum",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorEnum).GetField("Red")!),
("green", typeof(ColorEnum).GetField("Green")!),
("blue", typeof(ColorEnum).GetField("Blue")!)
    });
}