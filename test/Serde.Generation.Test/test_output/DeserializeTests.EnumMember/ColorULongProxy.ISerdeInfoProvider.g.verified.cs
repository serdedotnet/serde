//HintName: ColorULongProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial class ColorULongProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorULong",
    typeof(ColorULong).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetDeserializeInfo<ulong, global::Serde.U64Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("red", typeof(ColorULong).GetField("Red")),
        ("green", typeof(ColorULong).GetField("Green")),
        ("blue", typeof(ColorULong).GetField("Blue"))
    }
    );
}
