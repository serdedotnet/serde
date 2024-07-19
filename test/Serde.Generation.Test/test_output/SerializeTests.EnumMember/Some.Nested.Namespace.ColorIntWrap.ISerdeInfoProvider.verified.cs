//HintName: Some.Nested.Namespace.ColorIntWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial struct ColorIntWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorInt",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(Some.Nested.Namespace.ColorInt).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(Some.Nested.Namespace.ColorInt).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorIntWrap>(), typeof(Some.Nested.Namespace.ColorInt).GetField("Blue")!)
    });
}