//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("map", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<string, int>, Serde.DictProxy.Ser<string, int, global::Serde.StringProxy, global::Serde.I32Proxy>>(), typeof(C).GetField("Map"))
    }
    );
}