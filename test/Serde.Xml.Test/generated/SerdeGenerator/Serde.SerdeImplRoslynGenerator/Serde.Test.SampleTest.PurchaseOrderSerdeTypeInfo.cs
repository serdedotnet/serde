namespace Serde.Test;
partial class SampleTest
{
    internal static class PurchaseOrderSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("ShipTo", typeof(PurchaseOrder).GetField("ShipTo")!),
("OrderDate", typeof(PurchaseOrder).GetField("OrderDate")!),
("Items", typeof(PurchaseOrder).GetField("OrderedItems")!),
("SubTotal", typeof(PurchaseOrder).GetField("SubTotal")!),
("ShipCost", typeof(PurchaseOrder).GetField("ShipCost")!),
("TotalCost", typeof(PurchaseOrder).GetField("TotalCost")!)
    });
}
}