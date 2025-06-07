//HintName: Some.Nested.Namespace.ColorLongProxy.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial class ColorLongProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorLong",
    typeof(Some.Nested.Namespace.ColorLong).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetSerializeInfo<long, global::Serde.I64Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("red", typeof(Some.Nested.Namespace.ColorLong).GetField("Red")),
        ("green", typeof(Some.Nested.Namespace.ColorLong).GetField("Green")),
        ("blue", typeof(Some.Nested.Namespace.ColorLong).GetField("Blue"))
    }
    );
}
