//HintName: Some.Nested.Namespace.ColorULongProxy.ISerdeInfoProvider.g.cs

#nullable enable

namespace Some.Nested.Namespace;

partial class ColorULongProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorULong",
    typeof(Some.Nested.Namespace.ColorULong).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetSerializeInfo<ulong, global::Serde.U64Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("red", typeof(Some.Nested.Namespace.ColorULong).GetField("Red")),
        ("green", typeof(Some.Nested.Namespace.ColorULong).GetField("Green")),
        ("blue", typeof(Some.Nested.Namespace.ColorULong).GetField("Blue"))
    }
    );
}
