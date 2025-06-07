//HintName: ColorEnumProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial class ColorEnumProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorEnum",
    typeof(ColorEnum).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("Red", typeof(ColorEnum).GetField("Red")),
        ("Green", typeof(ColorEnum).GetField("Green")),
        ("Blue", typeof(ColorEnum).GetField("Blue"))
    }
    );
}
