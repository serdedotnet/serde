//HintName: ColorEnumSerdeTypeInfo.cs
internal static class ColorEnumSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorEnum",
        Serde.TypeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("Red", typeof(ColorEnum).GetField("Red")!),
("Green", typeof(ColorEnum).GetField("Green")!),
("Blue", typeof(ColorEnum).GetField("Blue")!)
    });
}