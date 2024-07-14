//HintName: S2SerdeInfo.cs
internal static class S2SerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "S2",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("E", typeof(S2).GetField("E")!)
    });
}