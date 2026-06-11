//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("s", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(C).GetField("S")),
        ("arr", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>(), typeof(C).GetField("Arr")),
        ("n", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(C).GetField("N"))
    }
    );
}
