//HintName: OptsWrap.ISerdeInfoProvider.cs

#nullable enable
partial record struct OptsWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "BIND_OPTS",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("cbStruct", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("cbStruct")!),
("dwTickCountDeadline", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("dwTickCountDeadline")!),
("grfFlags", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfFlags")!),
("grfMode", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfMode")!)
    });
}