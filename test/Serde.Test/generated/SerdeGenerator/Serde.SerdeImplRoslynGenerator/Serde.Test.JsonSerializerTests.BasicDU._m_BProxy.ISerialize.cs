
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_BProxy
        {
            sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B>
            {
                global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.BasicDU._m_BProxy.s_serdeInfo;

                void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B>.Serialize(Serde.Test.JsonSerializerTests.BasicDU.B value, global::Serde.ISerializer serializer)
                {
                    var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                    var _l_type = serializer.WriteType(_l_info);
                    _l_type.WriteString(_l_info, 0, value.Y);
                    _l_type.End(_l_info);
                }

            }
        }
    }
}
