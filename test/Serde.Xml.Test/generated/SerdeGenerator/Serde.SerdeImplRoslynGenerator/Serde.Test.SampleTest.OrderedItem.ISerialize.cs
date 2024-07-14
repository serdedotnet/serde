
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
                var _l_serdeInfo = OrderedItemSerdeInfo.Instance;
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<string, StringWrap>(_l_serdeInfo, 0, value.ItemName);
                type.SerializeField<string, StringWrap>(_l_serdeInfo, 1, value.Description);
                type.SerializeField<decimal, DecimalWrap>(_l_serdeInfo, 2, value.UnitPrice);
                type.SerializeField<int, Int32Wrap>(_l_serdeInfo, 3, value.Quantity);
                type.SerializeField<decimal, DecimalWrap>(_l_serdeInfo, 4, value.LineTotal);
                type.End();
            }
        }
    }
}