namespace Serde.Test;
partial class XmlTests
{
    internal static class NestedArraysSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("A", typeof(Serde.Test.XmlTests.NestedArrays).GetField("A")!)
    });
}
}