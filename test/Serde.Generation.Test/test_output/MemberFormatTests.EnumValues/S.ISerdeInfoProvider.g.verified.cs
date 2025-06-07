//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
    typeof(S).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("e", global::Serde.SerdeInfoProvider.GetSerializeInfo<ColorEnum, ColorEnumProxy>(), typeof(S).GetField("E"))
    }
    );
}
