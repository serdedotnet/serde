//HintName: ConstructedType.ISerdeInfoProvider.g.cs

#nullable enable
partial struct ConstructedType
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ConstructedType",
    typeof(ConstructedType).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("foreignType", global::Serde.SerdeInfoProvider.GetSerializeInfo<MyForeignType, MyForeignTypeProxy>(), typeof(ConstructedType).GetProperty("ForeignType"))
    }
    );
}
