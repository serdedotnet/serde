//HintName: Some.Nested.Namespace.ColorByteSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorByteSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ColorByte>(nameof(ColorByte), new (string, System.Reflection.MemberInfo)[] {
        ("red", typeof(ColorByte).GetField("Red")!),
("green", typeof(ColorByte).GetField("Green")!),
("blue", typeof(ColorByte).GetField("Blue")!)
    });
}