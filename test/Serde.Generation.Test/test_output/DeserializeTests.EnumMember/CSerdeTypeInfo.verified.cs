//HintName: CSerdeTypeInfo.cs
internal static class CSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("colorInt", typeof(C).GetField("ColorInt")!),
("colorByte", typeof(C).GetField("ColorByte")!),
("colorLong", typeof(C).GetField("ColorLong")!),
("colorULong", typeof(C).GetField("ColorULong")!)
    });
}