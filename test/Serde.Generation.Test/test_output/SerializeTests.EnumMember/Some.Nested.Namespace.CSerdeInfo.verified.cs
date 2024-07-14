//HintName: Some.Nested.Namespace.CSerdeInfo.cs
namespace Some.Nested.Namespace;
internal static class CSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "C",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("colorInt", typeof(Some.Nested.Namespace.C).GetField("ColorInt")!),
("colorByte", typeof(Some.Nested.Namespace.C).GetField("ColorByte")!),
("colorLong", typeof(Some.Nested.Namespace.C).GetField("ColorLong")!),
("colorULong", typeof(Some.Nested.Namespace.C).GetField("ColorULong")!)
    });
}