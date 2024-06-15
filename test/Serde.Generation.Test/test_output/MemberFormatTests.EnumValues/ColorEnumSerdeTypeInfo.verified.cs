//HintName: ColorEnumSerdeTypeInfo.cs
internal static class ColorEnumSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorEnum",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorEnum).GetField("Red")!),
("green", typeof(ColorEnum).GetField("Green")!),
("blue", typeof(ColorEnum).GetField("Blue")!)
    });
}