//HintName: Some.Nested.Namespace.ColorULongWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial struct ColorULongWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ColorULong",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<ColorULongWrap>(), typeof(Some.Nested.Namespace.ColorULong).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<ColorULongWrap>(), typeof(Some.Nested.Namespace.ColorULong).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<ColorULongWrap>(), typeof(Some.Nested.Namespace.ColorULong).GetField("Blue")!)
    });
}