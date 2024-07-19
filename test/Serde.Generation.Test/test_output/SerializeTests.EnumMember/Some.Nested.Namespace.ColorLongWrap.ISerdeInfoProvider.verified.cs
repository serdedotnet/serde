//HintName: Some.Nested.Namespace.ColorLongWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial struct ColorLongWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorLong",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorLongWrap>(), typeof(Some.Nested.Namespace.ColorLong).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorLongWrap>(), typeof(Some.Nested.Namespace.ColorLong).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorLongWrap>(), typeof(Some.Nested.Namespace.ColorLong).GetField("Blue")!)
    });
}