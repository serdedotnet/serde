
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record PurchaseOrder : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("PurchaseOrder", 6);
                type.SerializeField("ShipTo", this.ShipTo);
                type.SerializeField("OrderDate", new StringWrap(this.OrderDate));
                type.SerializeField("Items", new ArrayWrap.SerializeImpl<Serde.Test.SampleTest.OrderedItem, IdWrap<Serde.Test.SampleTest.OrderedItem>>(this.OrderedItems));
                type.SerializeField("SubTotal", new DecimalWrap(this.SubTotal));
                type.SerializeField("ShipCost", new DecimalWrap(this.ShipCost));
                type.SerializeField("TotalCost", new DecimalWrap(this.TotalCost));
                type.End();
            }
        }
    }
}