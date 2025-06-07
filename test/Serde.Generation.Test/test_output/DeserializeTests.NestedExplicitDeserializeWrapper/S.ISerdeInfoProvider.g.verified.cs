//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
    typeof(S).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("opts", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayProxy.De<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OptsWrap>>(), typeof(S).GetField("Opts"))
    }
    );
}
