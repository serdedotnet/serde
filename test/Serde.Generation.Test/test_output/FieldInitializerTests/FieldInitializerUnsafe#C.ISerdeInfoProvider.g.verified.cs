//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("items", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.List<int>, Serde.ListProxy.De<int, global::Serde.I32Proxy>>(), typeof(C).GetField("Items")),
        ("arr", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>(), typeof(C).GetField("Arr")),
        ("name", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(C).GetField("Name"))
    }
    );
}
