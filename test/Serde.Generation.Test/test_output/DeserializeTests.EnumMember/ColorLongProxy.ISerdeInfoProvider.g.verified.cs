//HintName: ColorLongProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial class ColorLongProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorLong",
    typeof(ColorLong).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetDeserializeInfo<long, global::Serde.I64Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("red", typeof(ColorLong).GetField("Red")),
        ("green", typeof(ColorLong).GetField("Green")),
        ("blue", typeof(ColorLong).GetField("Blue"))
    }
    );
}
