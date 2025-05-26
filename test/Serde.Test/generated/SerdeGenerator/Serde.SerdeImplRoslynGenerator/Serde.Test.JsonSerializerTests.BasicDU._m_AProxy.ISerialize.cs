
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_AProxy
        {
            sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.A>
            {
                global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.BasicDU._m_AProxy.s_serdeInfo;

                void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.A>.Serialize(Serde.Test.JsonSerializerTests.BasicDU.A value, global::Serde.ISerializer serializer)
                {
                    var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                    var _l_type = serializer.WriteType(_l_info);
                    _l_type.WriteI32(_l_info, 0, value.X);
                    _l_type.End(_l_info);
                }

            }
        }
    }
}
