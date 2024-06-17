//HintName: ColorByteSerdeTypeInfo.cs
internal static class ColorByteSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorByte).GetField("Red")!),
("green", typeof(ColorByte).GetField("Green")!),
("blue", typeof(ColorByte).GetField("Blue")!)
    });
}