//HintName: Some.Nested.Namespace.CSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class CSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "C",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("colorInt", typeof(Some.Nested.Namespace.C).GetField("ColorInt")!),
("colorByte", typeof(Some.Nested.Namespace.C).GetField("ColorByte")!),
("colorLong", typeof(Some.Nested.Namespace.C).GetField("ColorLong")!),
("colorULong", typeof(Some.Nested.Namespace.C).GetField("ColorULong")!)
    });
}