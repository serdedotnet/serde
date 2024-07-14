//HintName: PointSerdeInfo.cs
internal static class PointSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Point",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("x", typeof(Point).GetField("X")!),
("y", typeof(Point).GetField("Y")!)
    });
}