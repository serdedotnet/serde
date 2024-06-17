//HintName: C2SerdeTypeInfo.cs
internal static class C2SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("map", typeof(C2).GetField("Map")!)
    });
}