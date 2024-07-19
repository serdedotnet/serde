
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
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<PurchaseOrder>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<Serde.Test.SampleTest.Address, IdWrap<Serde.Test.SampleTest.Address>>(_l_serdeInfo, 0, value.ShipTo);
                type.SerializeField<string, global::Serde.StringWrap>(_l_serdeInfo, 1, value.OrderDate);
                type.SerializeField<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayWrap.SerializeImpl<Serde.Test.SampleTest.OrderedItem, global::Serde.IdWrap<Serde.Test.SampleTest.OrderedItem>>>(_l_serdeInfo, 2, value.OrderedItems);
                type.SerializeField<decimal, global::Serde.DecimalWrap>(_l_serdeInfo, 3, value.SubTotal);
                type.SerializeField<decimal, global::Serde.DecimalWrap>(_l_serdeInfo, 4, value.ShipCost);
                type.SerializeField<decimal, global::Serde.DecimalWrap>(_l_serdeInfo, 5, value.TotalCost);
                type.End();
            }
        }
    }
}