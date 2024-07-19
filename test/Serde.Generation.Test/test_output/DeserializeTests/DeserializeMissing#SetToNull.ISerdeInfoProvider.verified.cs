//HintName: SetToNull.ISerdeInfoProvider.cs

#nullable enable
partial record struct SetToNull : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "SetToNull",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(SetToNull).GetProperty("Present")!),
("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.DeserializeImpl<string,global::Serde.StringWrap>>(), typeof(SetToNull).GetProperty("Missing")!),
("throwMissing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.DeserializeImpl<string,global::Serde.StringWrap>>(), typeof(SetToNull).GetProperty("ThrowMissing")!)
    });
}