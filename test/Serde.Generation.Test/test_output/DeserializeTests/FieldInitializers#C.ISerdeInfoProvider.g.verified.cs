//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("str", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(C).GetField("Str")),
        ("num", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(C).GetField("Num")),
        ("flag", global::Serde.SerdeInfoProvider.GetDeserializeInfo<bool, global::Serde.BoolProxy>(), typeof(C).GetField("Flag")),
        ("nullable", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(C).GetField("Nullable")),
        ("fromMethod", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>(), typeof(C).GetField("FromMethod"))
    }
    );
}
