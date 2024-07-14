//HintName: SSerdeInfo.cs
internal static class SSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "S",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("fS", typeof(S<,,,,>).GetField("FS")!),
("f1", typeof(S<,,,,>).GetField("F1")!),
("f2", typeof(S<,,,,>).GetField("F2")!),
("f3", typeof(S<,,,,>).GetField("F3")!),
("f4", typeof(S<,,,,>).GetField("F4")!)
    });
}