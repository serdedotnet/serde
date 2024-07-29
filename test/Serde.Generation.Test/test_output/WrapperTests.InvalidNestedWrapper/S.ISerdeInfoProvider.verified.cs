//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("sections", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<System.Collections.Specialized.BitVector32.Section, Outer.SectionWrap>>(), typeof(S).GetField("Sections")!)
    });
}