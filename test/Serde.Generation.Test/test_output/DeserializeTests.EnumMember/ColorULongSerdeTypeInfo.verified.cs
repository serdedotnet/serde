//HintName: ColorULongSerdeTypeInfo.cs
internal static class ColorULongSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorULong",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorULong).GetField("Red")!),
("green", typeof(ColorULong).GetField("Green")!),
("blue", typeof(ColorULong).GetField("Blue")!)
    });
}