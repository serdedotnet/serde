//HintName: OptsWrap.ISerdeInfoProvider.g.cs

#nullable enable
partial record struct OptsWrap
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "BIND_OPTS",
    typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("cbStruct", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("cbStruct")),
        ("dwTickCountDeadline", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("dwTickCountDeadline")),
        ("grfFlags", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfFlags")),
        ("grfMode", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfMode"))
    }
    );
}
