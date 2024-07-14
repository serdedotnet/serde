//HintName: Some.Nested.Namespace.ColorULongSerdeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorULongSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorULong",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorULong).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorULong).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorULong).GetField("Blue")!)
    });
}