//HintName: PointSerdeTypeInfo.cs
internal static class PointSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "Point",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("x", typeof(Point).GetField("X")!),
("y", typeof(Point).GetField("Y")!)
    });
}