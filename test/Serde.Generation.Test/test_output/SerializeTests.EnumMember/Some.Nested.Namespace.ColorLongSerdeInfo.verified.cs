//HintName: Some.Nested.Namespace.ColorLongSerdeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorLongSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorLong",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorLong).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorLong).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorLong).GetField("Blue")!)
    });
}