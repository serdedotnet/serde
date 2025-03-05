
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SampleTest
{
    partial record PurchaseOrder : Serde.ISerializeProvider<Serde.Test.SampleTest.PurchaseOrder>
    {
        static ISerialize<Serde.Test.SampleTest.PurchaseOrder> ISerializeProvider<Serde.Test.SampleTest.PurchaseOrder>.SerializeInstance
            => PurchaseOrderSerializeProxy.Instance;

        sealed partial class PurchaseOrderSerializeProxy :Serde.ISerialize<Serde.Test.SampleTest.PurchaseOrder>
        {
            void global::Serde.ISerialize<Serde.Test.SampleTest.PurchaseOrder>.Serialize(Serde.Test.SampleTest.PurchaseOrder value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<PurchaseOrder>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteField<Serde.Test.SampleTest.Address, Serde.Test.SampleTest.Address>(_l_info, 0, value.ShipTo);
                _l_type.WriteString(_l_info, 1, value.OrderDate);
                _l_type.WriteField<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayProxy.Serialize<Serde.Test.SampleTest.OrderedItem,Serde.Test.SampleTest.OrderedItem>>(_l_info, 2, value.OrderedItems);
                _l_type.WriteDecimal(_l_info, 3, value.SubTotal);
                _l_type.WriteDecimal(_l_info, 4, value.ShipCost);
                _l_type.WriteDecimal(_l_info, 5, value.TotalCost);
                _l_type.End(_l_info);
            }
            public static readonly PurchaseOrderSerializeProxy Instance = new();
            private PurchaseOrderSerializeProxy() { }

        }
    }
}
