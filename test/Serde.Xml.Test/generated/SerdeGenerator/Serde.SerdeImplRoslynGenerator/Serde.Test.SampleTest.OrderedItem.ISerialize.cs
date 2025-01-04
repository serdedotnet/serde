
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SampleTest
{
    partial record OrderedItem : Serde.ISerializeProvider<Serde.Test.SampleTest.OrderedItem>
    {
        static ISerialize<Serde.Test.SampleTest.OrderedItem> ISerializeProvider<Serde.Test.SampleTest.OrderedItem>.SerializeInstance
            => OrderedItemSerializeProxy.Instance;

        sealed partial class OrderedItemSerializeProxy :Serde.ISerialize<Serde.Test.SampleTest.OrderedItem>
        {
            void global::Serde.ISerialize<Serde.Test.SampleTest.OrderedItem>.Serialize(Serde.Test.SampleTest.OrderedItem value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<OrderedItem>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,0,value.ItemName);
                type.SerializeField<string,global::Serde.StringProxy>(_l_serdeInfo,1,value.Description);
                type.SerializeField<decimal,global::Serde.DecimalProxy>(_l_serdeInfo,2,value.UnitPrice);
                type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,3,value.Quantity);
                type.SerializeField<decimal,global::Serde.DecimalProxy>(_l_serdeInfo,4,value.LineTotal);
                type.End();
            }
            public static readonly OrderedItemSerializeProxy Instance = new();
            private OrderedItemSerializeProxy() { }

        }
    }
}
