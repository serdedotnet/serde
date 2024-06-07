//HintName: SectionWrapSerdeTypeInfo.cs
internal static class SectionWrapSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("value", typeof(SectionWrap).GetProperty("Value")!)
    });
}