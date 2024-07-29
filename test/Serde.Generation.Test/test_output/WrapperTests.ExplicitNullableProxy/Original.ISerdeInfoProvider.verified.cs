//HintName: Original.ISerdeInfoProvider.cs

#nullable enable
partial record struct Original : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Original",
        typeof(Original).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("name", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Original).GetProperty("Name")!)
    });
}