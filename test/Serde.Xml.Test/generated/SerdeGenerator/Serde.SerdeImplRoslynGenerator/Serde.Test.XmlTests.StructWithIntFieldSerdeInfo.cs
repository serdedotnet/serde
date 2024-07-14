namespace Serde.Test;
partial class XmlTests
{
    internal static class StructWithIntFieldSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "StructWithIntField",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("X", typeof(Serde.Test.XmlTests.StructWithIntField).GetProperty("X")!)
    });
}
}