
#nullable enable
namespace Serde.Test;
partial class SampleTest
{
    partial record PurchaseOrder : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "PurchaseOrder",
        typeof(Serde.Test.SampleTest.PurchaseOrder).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("ShipTo", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.SampleTest.Address>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("ShipTo")!),
("OrderDate", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("OrderDate")!),
("Items", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<Serde.Test.SampleTest.OrderedItem,global::Serde.IdWrap<Serde.Test.SampleTest.OrderedItem>>>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("OrderedItems")!),
("SubTotal", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.DecimalWrap>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("SubTotal")!),
("ShipCost", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.DecimalWrap>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("ShipCost")!),
("TotalCost", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.DecimalWrap>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("TotalCost")!)
    });
}
}