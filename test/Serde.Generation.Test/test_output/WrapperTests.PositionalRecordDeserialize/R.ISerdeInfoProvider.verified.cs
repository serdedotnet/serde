//HintName: R.ISerdeInfoProvider.cs

#nullable enable
partial record R : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "R",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("a", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(R).GetProperty("A")!),
("b", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(R).GetProperty("B")!)
    });
}