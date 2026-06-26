//HintName: Wrap.ISerdeInfoProvider.g.cs

#nullable enable
partial record struct Wrap
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "BIND_OPTS",
        typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("cbStruct", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
            new("dwTickCountDeadline", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
            new("grfFlags", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
            new("grfMode", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
