//HintName: Proxy2.ISerdeInfoProvider.g.cs

#nullable enable
partial class Proxy2
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "BIND_OPTS",
    typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("cbStruct", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("cbStruct")),
        ("dwTickCountDeadline", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("dwTickCountDeadline")),
        ("grfFlags", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfFlags")),
        ("grfMode", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfMode"))
    }
    );
}
