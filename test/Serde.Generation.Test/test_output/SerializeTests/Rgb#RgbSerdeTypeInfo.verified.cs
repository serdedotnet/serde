//HintName: RgbSerdeTypeInfo.cs
internal static class RgbSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Rgb).GetField("Red")!),
("green", typeof(Rgb).GetField("Green")!),
("blue", typeof(Rgb).GetField("Blue")!)
    });
}