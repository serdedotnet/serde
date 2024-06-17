//HintName: ArrayFieldSerdeTypeInfo.cs
internal static class ArrayFieldSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("intArr", typeof(ArrayField).GetField("IntArr")!)
    });
}