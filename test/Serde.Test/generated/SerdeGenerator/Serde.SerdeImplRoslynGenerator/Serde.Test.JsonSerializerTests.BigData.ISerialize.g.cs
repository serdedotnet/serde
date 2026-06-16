
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BigData
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.BigData>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.BigData.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BigData>.Serialize(Serde.Test.JsonSerializerTests.BigData value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValue<System.Collections.Generic.List<int>, Serde.ListProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 0, value.Values);
                _l_type.End(_l_info);
            }

        }
    }
}
