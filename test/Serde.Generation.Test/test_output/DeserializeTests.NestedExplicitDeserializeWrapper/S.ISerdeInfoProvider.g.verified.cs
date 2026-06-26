//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("opts", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayProxy.De<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OptsWrap>>())
            {
                MemberInfo = typeof(S).GetField("Opts"),
            }
        }
    );
}
