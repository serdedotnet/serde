//HintName: ColorIntProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial class ColorIntProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorInt",
    typeof(ColorInt).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("red", typeof(ColorInt).GetField("Red")),
        ("green", typeof(ColorInt).GetField("Green")),
        ("blue", typeof(ColorInt).GetField("Blue"))
    }
    );
}
