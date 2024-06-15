//HintName: CSerdeTypeInfo.cs
internal static class CSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "C",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("nestedArr", typeof(C).GetField("NestedArr")!)
    });
}