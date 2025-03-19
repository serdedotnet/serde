
#nullable enable

namespace Serde.Test;

partial class SampleTest
{
    partial record PurchaseOrder
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "PurchaseOrder",
        typeof(Serde.Test.SampleTest.PurchaseOrder).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("ShipTo", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.SampleTest.Address, Serde.Test.SampleTest.Address>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("ShipTo")),
            ("OrderDate", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("OrderDate")),
            ("Items", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayProxy.Ser<Serde.Test.SampleTest.OrderedItem, Serde.Test.SampleTest.OrderedItem>>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("OrderedItems")),
            ("SubTotal", global::Serde.SerdeInfoProvider.GetSerializeInfo<decimal, global::Serde.DecimalProxy>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("SubTotal")),
            ("ShipCost", global::Serde.SerdeInfoProvider.GetSerializeInfo<decimal, global::Serde.DecimalProxy>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("ShipCost")),
            ("TotalCost", global::Serde.SerdeInfoProvider.GetSerializeInfo<decimal, global::Serde.DecimalProxy>(), typeof(Serde.Test.SampleTest.PurchaseOrder).GetField("TotalCost"))
        }
        );
    }
}