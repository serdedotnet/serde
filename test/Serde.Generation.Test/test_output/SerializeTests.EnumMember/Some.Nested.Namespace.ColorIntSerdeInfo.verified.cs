//HintName: Some.Nested.Namespace.ColorIntSerdeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorIntSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorInt",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorInt).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorInt).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorInt).GetField("Blue")!)
    });
}