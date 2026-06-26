//HintName: Other.ColorProxy.ISerdeInfoProvider.g.cs

#nullable enable

namespace Other;

partial class ColorProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "Color",
        typeof(Other.Color).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(),
        new (string, System.Reflection.MemberInfo?)[] {
            ("red", typeof(Other.Color).GetField("Red")),
            ("green", typeof(Other.Color).GetField("Green"))
        }
    );
}
