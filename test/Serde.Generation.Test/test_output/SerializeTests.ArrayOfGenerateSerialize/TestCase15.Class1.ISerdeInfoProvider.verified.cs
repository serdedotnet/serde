//HintName: TestCase15.Class1.ISerdeInfoProvider.cs

#nullable enable
partial class TestCase15
{
    partial class Class1 : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Class1",
        typeof(TestCase15.Class1).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("field0", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(TestCase15.Class1).GetField("Field0")!),
("field1", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(TestCase15.Class1).GetField("Field1")!)
    });
}
}