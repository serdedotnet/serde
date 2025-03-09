//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("f", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Deserialize<string, global::Serde.StringProxy>>(), typeof(S).GetField("F"))
        }
    );
}