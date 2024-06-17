//HintName: Some.Nested.Namespace.ColorByteSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorByteSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Some.Nested.Namespace.ColorByte).GetField("Red")!),
("green", typeof(Some.Nested.Namespace.ColorByte).GetField("Green")!),
("blue", typeof(Some.Nested.Namespace.ColorByte).GetField("Blue")!)
    });
}