//HintName: Some.Nested.Namespace.ColorByteProxy.ISerdeInfoProvider.cs

#nullable enable

namespace Some.Nested.Namespace;

partial class ColorByteProxy : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorByte",
        typeof(Some.Nested.Namespace.ColorByte).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.U8Proxy>(),
        new (string, System.Reflection.MemberInfo?)[] {
            ("red", typeof(Some.Nested.Namespace.ColorByte).GetField("Red")),
            ("green", typeof(Some.Nested.Namespace.ColorByte).GetField("Green")),
            ("blue", typeof(Some.Nested.Namespace.ColorByte).GetField("Blue"))
        }
    );
}