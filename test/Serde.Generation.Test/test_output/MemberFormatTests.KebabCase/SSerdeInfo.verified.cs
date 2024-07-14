//HintName: SSerdeInfo.cs
internal static class SSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "S",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("one", typeof(S).GetProperty("One")!),
("two-word", typeof(S).GetProperty("TwoWord")!)
    });
}