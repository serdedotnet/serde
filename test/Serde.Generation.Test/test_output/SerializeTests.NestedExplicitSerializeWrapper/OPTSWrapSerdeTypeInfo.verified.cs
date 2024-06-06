//HintName: OPTSWrapSerdeTypeInfo.cs
internal static class OPTSWrapSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<OPTSWrap>(nameof(OPTSWrap), new (string, System.Reflection.MemberInfo)[] {
        ("value", typeof(OPTSWrap).GetProperty("Value")!)
    });
}