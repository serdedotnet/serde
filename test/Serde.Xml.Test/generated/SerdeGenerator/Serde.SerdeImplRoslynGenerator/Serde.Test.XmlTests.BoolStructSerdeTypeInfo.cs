namespace Serde.Test;
partial class XmlTests
{
    internal static class BoolStructSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("BoolField", typeof(Serde.Test.XmlTests.BoolStruct).GetField("BoolField")!)
    });
}
}