//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "S",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("one", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(S).GetProperty("One")!),
("twoWord", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(S).GetProperty("TwoWord")!)
    });
}