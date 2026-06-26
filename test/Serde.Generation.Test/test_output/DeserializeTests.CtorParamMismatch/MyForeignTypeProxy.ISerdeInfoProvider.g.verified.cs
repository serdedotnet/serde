//HintName: MyForeignTypeProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial struct MyForeignTypeProxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "MyForeignType",
        typeof(MyForeignType).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("myInt", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
            new("myString", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
        }
    );
}
