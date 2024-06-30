//HintName: Some.Nested.Namespace.ColorULongSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorULongSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorULong",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorULong).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorULong).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorULong).GetField("Blue")!)
    });
}