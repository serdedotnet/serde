//HintName: Some.Nested.Namespace.ColorULongWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Some.Nested.Namespace;
partial struct ColorULongWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorULong",
        typeof(Some.Nested.Namespace.ColorULong).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt64Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorULong).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorULong).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorULong).GetField("Blue")!)
    });
}