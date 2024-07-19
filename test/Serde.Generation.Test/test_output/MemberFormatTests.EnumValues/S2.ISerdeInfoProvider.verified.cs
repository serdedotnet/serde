//HintName: S2.ISerdeInfoProvider.cs

#nullable enable
partial struct S2 : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "S2",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("E", global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>(), typeof(S2).GetField("E")!)
    });
}