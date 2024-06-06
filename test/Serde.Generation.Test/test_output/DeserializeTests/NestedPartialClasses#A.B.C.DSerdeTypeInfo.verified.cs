//HintName: A.B.C.DSerdeTypeInfo.cs
partial class A
{
    partial class B
{
    partial class C
{
    internal static class DSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<D>(nameof(D), new (string, System.Reflection.MemberInfo)[] {
        ("field", typeof(D).GetField("Field")!)
    });
}
}
}
}