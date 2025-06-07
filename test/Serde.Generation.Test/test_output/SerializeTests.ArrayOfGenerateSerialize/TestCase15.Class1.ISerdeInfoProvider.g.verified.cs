//HintName: TestCase15.Class1.ISerdeInfoProvider.g.cs

#nullable enable
partial class TestCase15
{
    partial class Class1
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Class1",
        typeof(TestCase15.Class1).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("field0", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(TestCase15.Class1).GetField("Field0")),
            ("field1", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte, global::Serde.U8Proxy>(), typeof(TestCase15.Class1).GetField("Field1"))
        }
        );
    }
}
