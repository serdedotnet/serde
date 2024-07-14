namespace Serde.Test;
partial class XmlTests
{
    internal static class NestedArraysSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "NestedArrays",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("A", typeof(Serde.Test.XmlTests.NestedArrays).GetField("A")!)
    });
}
}