
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
                var _l_typeInfo = PurchaseOrderSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<Serde.Test.SampleTest.Address, IdWrap<Serde.Test.SampleTest.Address>>(_l_typeInfo, 0, this.ShipTo);
                type.SerializeField<string, StringWrap>(_l_typeInfo, 1, this.OrderDate);
                type.SerializeField<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayWrap.SerializeImpl<Serde.Test.SampleTest.OrderedItem, IdWrap<Serde.Test.SampleTest.OrderedItem>>>(_l_typeInfo, 2, this.OrderedItems);
                type.SerializeField<decimal, DecimalWrap>(_l_typeInfo, 3, this.SubTotal);
                type.SerializeField<decimal, DecimalWrap>(_l_typeInfo, 4, this.ShipCost);
                type.SerializeField<decimal, DecimalWrap>(_l_typeInfo, 5, this.TotalCost);
                type.End();
            }
        }
    }
}