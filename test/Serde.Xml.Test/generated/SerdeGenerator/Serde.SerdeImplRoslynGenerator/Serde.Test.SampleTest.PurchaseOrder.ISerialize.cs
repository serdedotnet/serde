
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SampleTest
{
    partial record PurchaseOrder : Serde.ISerializeProvider<Serde.Test.SampleTest.PurchaseOrder>
    {
        static ISerialize<Serde.Test.SampleTest.PurchaseOrder> ISerializeProvider<Serde.Test.SampleTest.PurchaseOrder>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.SampleTest.PurchaseOrder>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SampleTest.PurchaseOrder.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.SampleTest.PurchaseOrder>.Serialize(Serde.Test.SampleTest.PurchaseOrder value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValue<Serde.Test.SampleTest.Address, Serde.Test.SampleTest.Address>(_l_info, 0, value.ShipTo);
                _l_type.WriteString(_l_info, 1, value.OrderDate);
                _l_type.WriteValue<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayProxy.Ser<Serde.Test.SampleTest.OrderedItem, Serde.Test.SampleTest.OrderedItem>>(_l_info, 2, value.OrderedItems);
                _l_type.WriteDecimal(_l_info, 3, value.SubTotal);
                _l_type.WriteDecimal(_l_info, 4, value.ShipCost);
                _l_type.WriteDecimal(_l_info, 5, value.TotalCost);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
