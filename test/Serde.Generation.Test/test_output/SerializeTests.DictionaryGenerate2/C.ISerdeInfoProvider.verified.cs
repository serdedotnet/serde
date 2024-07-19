//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial record C : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "C",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("x", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(C).GetProperty("X")!)
    });
}