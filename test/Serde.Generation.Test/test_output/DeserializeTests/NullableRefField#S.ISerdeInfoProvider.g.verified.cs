//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("f", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>())
        }
    );
}
