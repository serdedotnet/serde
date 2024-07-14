namespace Serde.Test;
partial class SampleTest
{
    internal static class AddressSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "Address",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("Name", typeof(Serde.Test.SampleTest.Address).GetField("Name")!),
("Line1", typeof(Serde.Test.SampleTest.Address).GetField("Line1")!),
("City", typeof(Serde.Test.SampleTest.Address).GetField("City")!),
("State", typeof(Serde.Test.SampleTest.Address).GetField("State")!),
("Zip", typeof(Serde.Test.SampleTest.Address).GetField("Zip")!)
    });
}
}