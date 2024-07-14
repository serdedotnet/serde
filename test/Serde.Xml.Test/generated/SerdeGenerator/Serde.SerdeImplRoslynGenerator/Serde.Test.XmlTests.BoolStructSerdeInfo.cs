namespace Serde.Test;
partial class XmlTests
{
    internal static class BoolStructSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "BoolStruct",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("BoolField", typeof(Serde.Test.XmlTests.BoolStruct).GetField("BoolField")!)
    });
}
}