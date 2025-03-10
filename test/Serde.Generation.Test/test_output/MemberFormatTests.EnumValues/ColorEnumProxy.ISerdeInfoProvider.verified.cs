//HintName: ColorEnumProxy.ISerdeInfoProvider.cs

#nullable enable
partial class ColorEnumProxy : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorEnum",
        typeof(ColorEnum).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(),
        new (string, System.Reflection.MemberInfo?)[] {
            ("red", typeof(ColorEnum).GetField("Red")),
            ("green", typeof(ColorEnum).GetField("Green")),
            ("blue", typeof(ColorEnum).GetField("Blue"))
        }
    );
}