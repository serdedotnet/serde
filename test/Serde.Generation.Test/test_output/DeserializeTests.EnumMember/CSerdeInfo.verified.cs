//HintName: CSerdeInfo.cs
internal static class CSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "C",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("colorInt", typeof(C).GetField("ColorInt")!),
("colorByte", typeof(C).GetField("ColorByte")!),
("colorLong", typeof(C).GetField("ColorLong")!),
("colorULong", typeof(C).GetField("ColorULong")!)
    });
}