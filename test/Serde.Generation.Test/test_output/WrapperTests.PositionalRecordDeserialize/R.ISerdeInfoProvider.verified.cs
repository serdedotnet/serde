//HintName: R.ISerdeInfoProvider.cs

#nullable enable
partial record R : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "R",
        typeof(R).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("a", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(), typeof(R).GetProperty("A")),
            ("b", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(R).GetProperty("B"))
        }
    );
}