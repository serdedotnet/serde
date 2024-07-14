//HintName: C2SerdeInfo.cs
internal static class C2SerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "C2",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("map", typeof(C2).GetField("Map")!)
    });
}