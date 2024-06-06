//HintName: C2SerdeTypeInfo.cs
internal static class C2SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<C2>(nameof(C2), new (string, System.Reflection.MemberInfo)[] {
        ("map", typeof(C2).GetField("Map")!)
    });
}