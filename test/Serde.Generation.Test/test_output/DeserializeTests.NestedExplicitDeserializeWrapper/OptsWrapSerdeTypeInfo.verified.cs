//HintName: OptsWrapSerdeTypeInfo.cs
internal static class OptsWrapSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("value", typeof(OptsWrap).GetProperty("Value")!)
    });
}