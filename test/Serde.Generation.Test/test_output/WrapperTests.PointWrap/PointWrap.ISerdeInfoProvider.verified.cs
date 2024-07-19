//HintName: PointWrap.ISerdeInfoProvider.cs

#nullable enable
partial struct PointWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Point",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("x", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Point).GetField("X")!),
("y", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Point).GetField("Y")!)
    });
}