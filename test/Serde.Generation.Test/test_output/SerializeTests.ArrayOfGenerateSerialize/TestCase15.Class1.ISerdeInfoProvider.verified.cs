//HintName: TestCase15.Class1.ISerdeInfoProvider.cs

#nullable enable
partial class TestCase15
{
    partial class Class1 : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Class1",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("field0", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(TestCase15.Class1).GetField("Field0")!),
("field1", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(TestCase15.Class1).GetField("Field1")!)
    });
}
}