//HintName: Some.Nested.Namespace.ColorIntSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorIntSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ColorInt>(nameof(ColorInt), new (string, System.Reflection.MemberInfo)[] {
        ("red", typeof(ColorInt).GetField("Red")!),
("green", typeof(ColorInt).GetField("Green")!),
("blue", typeof(ColorInt).GetField("Blue")!)
    });
}