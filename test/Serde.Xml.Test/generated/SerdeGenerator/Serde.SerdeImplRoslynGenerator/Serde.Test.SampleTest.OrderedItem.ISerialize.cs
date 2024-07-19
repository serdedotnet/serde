
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
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<OrderedItem>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 0, value.ItemName);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 1, value.Description);
                type.SerializeField<decimal, global::Serde.DecimalWrap>(_l_serdeInfo, 2, value.UnitPrice);
                type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 3, value.Quantity);
                type.SerializeField<decimal, global::Serde.DecimalWrap>(_l_serdeInfo, 4, value.LineTotal);
                type.End();
            }
        }
    }
}