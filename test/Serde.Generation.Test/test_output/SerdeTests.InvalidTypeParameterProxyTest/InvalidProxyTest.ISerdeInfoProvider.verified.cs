//HintName: InvalidProxyTest.ISerdeInfoProvider.cs

#nullable enable
partial class InvalidProxyTest<T>
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "InvalidProxyTest",
    typeof(InvalidProxyTest<>).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {

    }
    );
}
