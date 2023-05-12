
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record OrderedItem : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("OrderedItem", 5);
                type.SerializeField("ItemName"u8, new StringWrap(this.ItemName));
                type.SerializeField("Description"u8, new StringWrap(this.Description));
                type.SerializeField("UnitPrice"u8, new DecimalWrap(this.UnitPrice));
                type.SerializeField("Quantity"u8, new Int32Wrap(this.Quantity));
                type.SerializeField("LineTotal"u8, new DecimalWrap(this.LineTotal));
                type.End();
            }
        }
    }
}