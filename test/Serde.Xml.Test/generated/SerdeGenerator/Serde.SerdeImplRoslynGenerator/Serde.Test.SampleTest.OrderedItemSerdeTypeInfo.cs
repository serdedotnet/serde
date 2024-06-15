namespace Serde.Test;
partial class SampleTest
{
    internal static class OrderedItemSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "OrderedItem",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("ItemName", typeof(Serde.Test.SampleTest.OrderedItem).GetField("ItemName")!),
("Description", typeof(Serde.Test.SampleTest.OrderedItem).GetField("Description")!),
("UnitPrice", typeof(Serde.Test.SampleTest.OrderedItem).GetField("UnitPrice")!),
("Quantity", typeof(Serde.Test.SampleTest.OrderedItem).GetField("Quantity")!),
("LineTotal", typeof(Serde.Test.SampleTest.OrderedItem).GetField("LineTotal")!)
    });
}
}