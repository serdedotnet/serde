//HintName: TestCase15.Class0.ISerdeInfoProvider.g.cs

#nullable enable
partial class TestCase15
{
    partial class Class0
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Class0",
            typeof(TestCase15.Class0).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("field0", global::Serde.SerdeInfoProvider.GetSerializeInfo<TestCase15.Class1[], Serde.ArrayProxy.Ser<TestCase15.Class1, TestCase15.Class1>>()),
                new("field1", global::Serde.SerdeInfoProvider.GetSerializeInfo<bool[], Serde.ArrayProxy.Ser<bool, global::Serde.BoolProxy>>())
            }
        );
    }
}
