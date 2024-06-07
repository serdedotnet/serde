//HintName: WrapSerdeTypeInfo.cs
internal static class WrapSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("value", typeof(Wrap).GetProperty("Value")!)
    });
}