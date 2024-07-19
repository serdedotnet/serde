//HintName: SectionWrap.ISerdeInfoProvider.cs

#nullable enable
partial record struct SectionWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Section",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("mask", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int16Wrap>(), typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Mask")!),
("offset", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int16Wrap>(), typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Offset")!)
    });
}