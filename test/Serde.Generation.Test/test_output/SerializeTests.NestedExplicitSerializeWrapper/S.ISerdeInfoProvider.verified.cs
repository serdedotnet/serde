//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("opts", global::Serde.SerdeInfoProvider.GetInfo<Serde.ImmutableArrayWrap.SerializeImpl<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>>(), typeof(S).GetField("Opts")!)
    });
}