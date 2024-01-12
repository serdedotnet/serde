
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record OrderedItem : Serde.ISerialize<Serde.Test.SampleTest.OrderedItem>
        {
            void ISerialize<Serde.Test.SampleTest.OrderedItem>.Serialize(Serde.Test.SampleTest.OrderedItem value, ISerializer serializer)
            {
                var type = serializer.SerializeType("OrderedItem", 5);
                type.SerializeField<string, StringWrap>("ItemName", value.ItemName);
                type.SerializeField<string, StringWrap>("Description", value.Description);
                type.SerializeField<decimal, DecimalWrap>("UnitPrice", value.UnitPrice);
                type.SerializeField<int, Int32Wrap>("Quantity", value.Quantity);
                type.SerializeField<decimal, DecimalWrap>("LineTotal", value.LineTotal);
                type.End();
            }
        }
    }
}