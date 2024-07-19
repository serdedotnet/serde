//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "S",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("opts", global::Serde.SerdeInfoProvider.GetInfo<Serde.ImmutableArrayWrap.SerializeImpl<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>>(), typeof(S).GetField("Opts")!)
    });
}