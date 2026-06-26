//HintName: C2.ISerdeInfoProvider.g.cs

#nullable enable
partial class C2
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C2",
        typeof(C2).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("map", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.Dictionary<string, C>, Serde.DictProxy.Ser<string, C, global::Serde.StringProxy, C>>())
        }
    );
}
