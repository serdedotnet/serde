//HintName: RSerdeInfo.cs
internal static class RSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "R",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("a", typeof(R).GetProperty("A")!),
("b", typeof(R).GetProperty("B")!)
    });
}