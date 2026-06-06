//HintName: ForeignPointProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial struct ForeignPointProxy
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ForeignPoint",
    typeof(ForeignPointProxy).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(ForeignPointProxy).GetField("X")),
        ("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(ForeignPointProxy).GetField("Y"))
    }
    );
}
