//HintName: SSerdeTypeInfo.cs
internal static class SSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "S",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("fI", typeof(S<,,>).GetField("FI")!),
("f1", typeof(S<,,>).GetField("F1")!),
("f2", typeof(S<,,>).GetField("F2")!),
("f3", typeof(S<,,>).GetField("F3")!)
    });
}