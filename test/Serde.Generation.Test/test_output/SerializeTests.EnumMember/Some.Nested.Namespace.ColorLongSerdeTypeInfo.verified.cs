//HintName: Some.Nested.Namespace.ColorLongSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorLongSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorLong",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorLong).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorLong).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorLong).GetField("Blue")!)
    });
}