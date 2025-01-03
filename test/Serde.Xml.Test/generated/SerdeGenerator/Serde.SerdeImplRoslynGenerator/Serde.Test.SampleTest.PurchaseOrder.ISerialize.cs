
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record PurchaseOrder : Serde.ISerializeProvider<Serde.Test.SampleTest.PurchaseOrder>
        {
            static ISerialize<Serde.Test.SampleTest.PurchaseOrder> ISerializeProvider<Serde.Test.SampleTest.PurchaseOrder>.SerializeInstance => PurchaseOrderSerializeProxy.Instance;

            sealed class PurchaseOrderSerializeProxy : Serde.ISerialize<Serde.Test.SampleTest.PurchaseOrder>
            {
                void ISerialize<Serde.Test.SampleTest.PurchaseOrder>.Serialize(Serde.Test.SampleTest.PurchaseOrder value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<PurchaseOrder>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<Serde.Test.SampleTest.Address, Serde.Test.SampleTest.Address>(_l_serdeInfo, 0, value.ShipTo);
                    type.SerializeField<string, global::Serde.StringProxy>(_l_serdeInfo, 1, value.OrderDate);
                    type.SerializeField<Serde.Test.SampleTest.OrderedItem[], Serde.ArrayProxy.Serialize<Serde.Test.SampleTest.OrderedItem, Serde.Test.SampleTest.OrderedItem>>(_l_serdeInfo, 2, value.OrderedItems);
                    type.SerializeField<decimal, global::Serde.DecimalProxy>(_l_serdeInfo, 3, value.SubTotal);
                    type.SerializeField<decimal, global::Serde.DecimalProxy>(_l_serdeInfo, 4, value.ShipCost);
                    type.SerializeField<decimal, global::Serde.DecimalProxy>(_l_serdeInfo, 5, value.TotalCost);
                    type.End();
                }

                public static readonly PurchaseOrderSerializeProxy Instance = new();
                private PurchaseOrderSerializeProxy()
                {
                }
            }
        }
    }
}