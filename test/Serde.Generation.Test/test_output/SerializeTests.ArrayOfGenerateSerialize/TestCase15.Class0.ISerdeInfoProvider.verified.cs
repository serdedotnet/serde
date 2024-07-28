//HintName: TestCase15.Class0.ISerdeInfoProvider.cs

#nullable enable
partial class TestCase15
{
    partial class Class0 : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Class0",
        typeof(TestCase15.Class0).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("field0", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<TestCase15.Class1,global::Serde.IdWrap<TestCase15.Class1>>>(), typeof(TestCase15.Class0).GetField("Field0")!),
("field1", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<bool,global::Serde.BoolWrap>>(), typeof(TestCase15.Class0).GetField("Field1")!)
    });
}
}