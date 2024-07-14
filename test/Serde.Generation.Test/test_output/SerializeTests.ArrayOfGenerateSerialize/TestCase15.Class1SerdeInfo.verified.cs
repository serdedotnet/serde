//HintName: TestCase15.Class1SerdeInfo.cs
partial class TestCase15
{
    internal static class Class1SerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Class1",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("field0", typeof(TestCase15.Class1).GetField("Field0")!),
("field1", typeof(TestCase15.Class1).GetField("Field1")!)
    });
}
}