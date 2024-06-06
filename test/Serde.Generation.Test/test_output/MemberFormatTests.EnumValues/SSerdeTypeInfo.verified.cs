//HintName: SSerdeTypeInfo.cs
internal static class SSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<S>(nameof(S), new (string, System.Reflection.MemberInfo)[] {
        ("e", typeof(S).GetField("E")!)
    });
}