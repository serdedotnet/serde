//HintName: ColorEnumSerdeTypeInfo.cs
internal static class ColorEnumSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("Red", typeof(ColorEnum).GetField("Red")!),
("Green", typeof(ColorEnum).GetField("Green")!),
("Blue", typeof(ColorEnum).GetField("Blue")!)
    });
}