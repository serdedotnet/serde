//HintName: Some.Nested.Namespace.ColorByteWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial struct ColorByteWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorByte",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(Some.Nested.Namespace.ColorByte).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(Some.Nested.Namespace.ColorByte).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorByteWrap>(), typeof(Some.Nested.Namespace.ColorByte).GetField("Blue")!)
    });
}