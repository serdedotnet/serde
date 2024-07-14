//HintName: CSerdeInfo.cs
internal static class CSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "C",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("intArr", typeof(C).GetField("IntArr")!)
    });
}