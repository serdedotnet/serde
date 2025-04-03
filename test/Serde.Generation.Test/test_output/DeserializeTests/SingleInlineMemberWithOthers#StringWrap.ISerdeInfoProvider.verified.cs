//HintName: StringWrap.ISerdeInfoProvider.cs

#nullable enable
partial record StringWrap
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo
        = global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>();
}