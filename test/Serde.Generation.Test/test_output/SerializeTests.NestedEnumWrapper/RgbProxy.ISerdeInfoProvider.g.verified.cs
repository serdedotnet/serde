//HintName: RgbProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial class RgbProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "Rgb",
    typeof(Rgb).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("red", typeof(Rgb).GetField("Red")),
        ("green", typeof(Rgb).GetField("Green")),
        ("blue", typeof(Rgb).GetField("Blue"))
    }
    );
}
