namespaceSerde.Test{partialclassSampleTest{internal static class OrderedItemSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<OrderedItem>(nameof(OrderedItem), new (string, System.Reflection.MemberInfo)[] {
        ("ItemName", typeof(OrderedItem).GetField("ItemName")!),
("Description", typeof(OrderedItem).GetField("Description")!),
("UnitPrice", typeof(OrderedItem).GetField("UnitPrice")!),
("Quantity", typeof(OrderedItem).GetField("Quantity")!),
("LineTotal", typeof(OrderedItem).GetField("LineTotal")!)
    });
}}}