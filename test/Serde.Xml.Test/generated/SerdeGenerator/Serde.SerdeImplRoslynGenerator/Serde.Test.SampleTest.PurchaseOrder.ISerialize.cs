
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
                type.SerializeField("ShipTo"u8, this.ShipTo);
                type.SerializeField("OrderDate"u8, new StringWrap(this.OrderDate));
                type.SerializeField("Items"u8, new ArrayWrap.SerializeImpl<Serde.Test.SampleTest.OrderedItem, IdWrap<Serde.Test.SampleTest.OrderedItem>>(this.OrderedItems));
                type.SerializeField("SubTotal"u8, new DecimalWrap(this.SubTotal));
                type.SerializeField("ShipCost"u8, new DecimalWrap(this.ShipCost));
                type.SerializeField("TotalCost"u8, new DecimalWrap(this.TotalCost));
                type.End();
            }
        }
    }
}