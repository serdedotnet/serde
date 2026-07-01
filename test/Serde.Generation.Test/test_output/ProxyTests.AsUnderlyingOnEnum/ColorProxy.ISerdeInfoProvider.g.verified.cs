//HintName: ColorProxy.ISerdeInfoProvider.g.cs

#nullable enable
partial class ColorProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "Color",
        typeof(Color).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(),
        new (string, System.Reflection.MemberInfo?)[] {
            ("red", typeof(Color).GetField("Red")),
            ("green", typeof(Color).GetField("Green")),
            ("blue", typeof(Color).GetField("Blue"))
        }
    );
}
