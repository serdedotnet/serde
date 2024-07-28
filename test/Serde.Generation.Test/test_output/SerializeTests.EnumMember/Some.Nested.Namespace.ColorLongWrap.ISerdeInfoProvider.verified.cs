//HintName: Some.Nested.Namespace.ColorLongWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial struct ColorLongWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorLong",
        typeof(Some.Nested.Namespace.ColorLong).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int64Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorLong).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorLong).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorLong).GetField("Blue")!)
    });
}