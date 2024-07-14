namespace Serde.Test;
partial class XmlTests
{
    internal static class TypeWithArrayFieldSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "TypeWithArrayField",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("ArrayField", typeof(Serde.Test.XmlTests.TypeWithArrayField).GetField("ArrayField")!)
    });
}
}