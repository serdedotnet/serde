//HintName: SectionSerdeTypeInfo.cs
internal static class SectionSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "Section",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("mask", typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Mask")!),
("offset", typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Offset")!)
    });
}