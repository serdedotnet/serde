//HintName: Test.ISerdeInfoProvider.g.cs

#nullable enable
partial class Test
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Test",
    typeof(Test).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("data", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<EqArray<int>, int>, Serde.DictProxy.Ser<EqArray<int>, int, EqArrayProxy.Ser<int, global::Serde.I32Proxy>, global::Serde.I32Proxy>>(), typeof(Test).GetField("data"))
    }
    );
}
