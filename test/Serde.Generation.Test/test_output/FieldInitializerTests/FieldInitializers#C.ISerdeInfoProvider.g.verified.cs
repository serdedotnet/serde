//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("str", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
            {
                IsOptional = true,
            },
            new("num", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            {
                IsOptional = true,
            },
            new("flag", global::Serde.SerdeInfoProvider.GetDeserializeInfo<bool, global::Serde.BoolProxy>())
            {
                IsOptional = true,
            },
            new("nullable", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>())
            {
                IsOptional = true,
            },
            new("dbl", global::Serde.SerdeInfoProvider.GetDeserializeInfo<double, global::Serde.F64Proxy>())
            {
                IsOptional = true,
            },
            new("ch", global::Serde.SerdeInfoProvider.GetDeserializeInfo<char, global::Serde.CharProxy>())
            {
                IsOptional = true,
            },
            new("fromMethod", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
            {
                IsOptional = true,
            },
            new("maxInt", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
            {
                IsOptional = true,
            }
        }
    );
}
