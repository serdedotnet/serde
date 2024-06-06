//HintName: Some.Nested.Namespace.CSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class CSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<C>(nameof(C), new (string, System.Reflection.MemberInfo)[] {
        ("colorInt", typeof(C).GetField("ColorInt")!),
("colorByte", typeof(C).GetField("ColorByte")!),
("colorLong", typeof(C).GetField("ColorLong")!),
("colorULong", typeof(C).GetField("ColorULong")!)
    });
}