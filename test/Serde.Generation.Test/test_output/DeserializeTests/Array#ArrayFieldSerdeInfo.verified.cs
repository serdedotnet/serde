//HintName: ArrayFieldSerdeInfo.cs
internal static class ArrayFieldSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ArrayField",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("intArr", typeof(ArrayField).GetField("IntArr")!)
    });
}