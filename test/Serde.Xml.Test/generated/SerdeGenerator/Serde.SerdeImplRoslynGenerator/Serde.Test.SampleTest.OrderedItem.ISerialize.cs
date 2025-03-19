
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SampleTest
{
    partial record OrderedItem : Serde.ISerializeProvider<Serde.Test.SampleTest.OrderedItem>
    {
        static ISerialize<Serde.Test.SampleTest.OrderedItem> ISerializeProvider<Serde.Test.SampleTest.OrderedItem>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.SampleTest.OrderedItem>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SampleTest.OrderedItem.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.SampleTest.OrderedItem>.Serialize(Serde.Test.SampleTest.OrderedItem value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteString(_l_info, 0, value.ItemName);
                _l_type.WriteString(_l_info, 1, value.Description);
                _l_type.WriteDecimal(_l_info, 2, value.UnitPrice);
                _l_type.WriteI32(_l_info, 3, value.Quantity);
                _l_type.WriteDecimal(_l_info, 4, value.LineTotal);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
