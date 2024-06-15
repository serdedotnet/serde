//HintName: Outer.SectionSerdeTypeInfo.cs
partial class Outer
{
    internal static class SectionSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("mask", typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Mask")!),
("offset", typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Offset")!)
    });
}
}