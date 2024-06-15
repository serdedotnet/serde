namespace Serde.Test;
partial class XmlTests
{
    internal static class StructWithIntFieldSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "StructWithIntField",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("X", typeof(Serde.Test.XmlTests.StructWithIntField).GetProperty("X")!)
    });
}
}