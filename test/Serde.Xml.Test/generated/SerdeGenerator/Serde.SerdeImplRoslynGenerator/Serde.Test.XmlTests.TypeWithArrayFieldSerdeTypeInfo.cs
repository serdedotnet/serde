namespace Serde.Test;
partial class XmlTests
{
    internal static class TypeWithArrayFieldSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("ArrayField", typeof(TypeWithArrayField).GetField("ArrayField")!)
    });
}
}