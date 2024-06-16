//HintName: S1SerdeTypeInfo.cs
internal static class S1SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("x", typeof(S1).GetField("X")!)
    });
}