namespace Serde.Test;
partial class SampleTest
{
    internal static class AddressSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("Name", typeof(Serde.Test.SampleTest.Address).GetField("Name")!),
("Line1", typeof(Serde.Test.SampleTest.Address).GetField("Line1")!),
("City", typeof(Serde.Test.SampleTest.Address).GetField("City")!),
("State", typeof(Serde.Test.SampleTest.Address).GetField("State")!),
("Zip", typeof(Serde.Test.SampleTest.Address).GetField("Zip")!)
    });
}
}