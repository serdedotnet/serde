//HintName: RSerdeTypeInfo.cs
internal static class RSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<R>(nameof(R), new (string, System.Reflection.MemberInfo)[] {
        ("a", typeof(R).GetProperty("A")!),
("b", typeof(R).GetProperty("B")!)
    });
}