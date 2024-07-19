//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "S",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("sections", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(S).GetField("Sections")!)
    });
}