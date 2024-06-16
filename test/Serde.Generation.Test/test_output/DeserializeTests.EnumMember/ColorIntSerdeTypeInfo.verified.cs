//HintName: ColorIntSerdeTypeInfo.cs
internal static class ColorIntSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorInt).GetField("Red")!),
("green", typeof(ColorInt).GetField("Green")!),
("blue", typeof(ColorInt).GetField("Blue")!)
    });
}