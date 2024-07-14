//HintName: Some.Nested.Namespace.ColorByteSerdeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorByteSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "ColorByte",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorByte).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorByte).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorByte).GetField("Blue")!)
    });
}