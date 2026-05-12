//HintName: MyForeignTypeProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial record struct MyForeignTypeProxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "MyForeignType",
    typeof(MyForeignType).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("myInt", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(MyForeignType).GetProperty("MyInt")),
        ("myString", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(MyForeignType).GetProperty("MyString"))
    }
    );
}
