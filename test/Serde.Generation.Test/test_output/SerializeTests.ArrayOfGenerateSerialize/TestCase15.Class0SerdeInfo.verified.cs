//HintName: TestCase15.Class0SerdeInfo.cs
partial class TestCase15
{
    internal static class Class0SerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Class0",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("field0", typeof(TestCase15.Class0).GetField("Field0")!),
("field1", typeof(TestCase15.Class0).GetField("Field1")!)
    });
}
}