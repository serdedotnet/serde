//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
    typeof(S).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("opts", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayProxy.Ser<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>>(), typeof(S).GetField("Opts"))
    }
    );
}
