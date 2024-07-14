//HintName: A.B.C.DSerdeInfo.cs
partial class A
{
    partial class B
{
    partial class C
{
    internal static class DSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "D",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("field", typeof(A.B.C.D).GetField("Field")!)
    });
}
}
}
}