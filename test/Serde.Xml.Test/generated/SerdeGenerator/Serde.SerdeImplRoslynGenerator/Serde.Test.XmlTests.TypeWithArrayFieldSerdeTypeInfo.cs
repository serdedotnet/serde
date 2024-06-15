namespace Serde.Test;
partial class XmlTests
{
    internal static class TypeWithArrayFieldSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "TypeWithArrayField",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("ArrayField", typeof(Serde.Test.XmlTests.TypeWithArrayField).GetField("ArrayField")!)
    });
}
}