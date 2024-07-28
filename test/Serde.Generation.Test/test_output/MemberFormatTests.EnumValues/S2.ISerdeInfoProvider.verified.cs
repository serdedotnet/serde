//HintName: S2.ISerdeInfoProvider.cs

#nullable enable
partial struct S2 : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S2",
        typeof(S2).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("E", global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>(), typeof(S2).GetField("E")!)
    });
}