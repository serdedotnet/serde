namespace Serde.Test;
partial class SampleTest
{
    internal static class PurchaseOrderSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "PurchaseOrder",
        Serde.TypeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("ShipTo", typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("ShipTo")!),
("OrderDate", typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("OrderDate")!),
("Items", typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("OrderedItems")!),
("SubTotal", typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("SubTotal")!),
("ShipCost", typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("ShipCost")!),
("TotalCost", typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("TotalCost")!)
    });
}
}