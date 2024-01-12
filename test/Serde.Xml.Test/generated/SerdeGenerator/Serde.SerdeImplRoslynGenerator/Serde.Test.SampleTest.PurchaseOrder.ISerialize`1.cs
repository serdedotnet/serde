
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record PurchaseOrder : Serde.ISerialize<Serde.Test.SampleTest.PurchaseOrder>
        {
            void ISerialize<Serde.Test.SampleTest.PurchaseOrder>.Serialize(Serde.Test.SampleTest.PurchaseOrder value, ISerializer serializer)
            {
                var type = serializer.SerializeType("PurchaseOrder", 6);
                type.SerializeField<Serde.Test.SampleTest.Address, IdWrap<Serde.Test.SampleTest.Address>>("ShipTo", value.ShipTo);
                type.SerializeField<string, StringWrap>("OrderDate", value.OrderDate);
                type.SerializeField<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayWrap.SerializeImpl<Serde.Test.SampleTest.OrderedItem, IdWrap<Serde.Test.SampleTest.OrderedItem>>>("Items", value.OrderedItems);
                type.SerializeField<decimal, DecimalWrap>("SubTotal", value.SubTotal);
                type.SerializeField<decimal, DecimalWrap>("ShipCost", value.ShipCost);
                type.SerializeField<decimal, DecimalWrap>("TotalCost", value.TotalCost);
                type.End();
            }
        }
    }
}