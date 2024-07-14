//HintName: ColorByteSerdeInfo.cs
internal static class ColorByteSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorByte",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorByte).GetField("Red")!),
("green", typeof(ColorByte).GetField("Green")!),
("blue", typeof(ColorByte).GetField("Blue")!)
    });
}