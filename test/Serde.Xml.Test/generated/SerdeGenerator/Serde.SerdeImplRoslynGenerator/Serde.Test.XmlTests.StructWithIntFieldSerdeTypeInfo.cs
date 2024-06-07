namespace Serde.Test;
partial class XmlTests
{
    internal static class StructWithIntFieldSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("X", typeof(StructWithIntField).GetProperty("X")!)
    });
}
}