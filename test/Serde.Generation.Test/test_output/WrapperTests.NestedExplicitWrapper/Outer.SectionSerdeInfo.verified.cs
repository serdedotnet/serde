//HintName: Outer.SectionSerdeInfo.cs
partial class Outer
{
    internal static class SectionSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Section",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("mask", typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Mask")!),
("offset", typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Offset")!)
    });
}
}