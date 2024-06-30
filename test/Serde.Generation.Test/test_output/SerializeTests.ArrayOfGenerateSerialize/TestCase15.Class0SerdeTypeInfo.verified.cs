//HintName: TestCase15.Class0SerdeTypeInfo.cs
partial class TestCase15
{
    internal static class Class0SerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "Class0",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("field0", typeof(TestCase15.Class0).GetField("Field0")!),
("field1", typeof(TestCase15.Class0).GetField("Field1")!)
    });
}
}