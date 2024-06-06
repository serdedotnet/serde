//HintName: Some.Nested.Namespace.ColorLongSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorLongSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ColorLong>(nameof(ColorLong), new (string, System.Reflection.MemberInfo)[] {
        ("red", typeof(ColorLong).GetField("Red")!),
("green", typeof(ColorLong).GetField("Green")!),
("blue", typeof(ColorLong).GetField("Blue")!)
    });
}