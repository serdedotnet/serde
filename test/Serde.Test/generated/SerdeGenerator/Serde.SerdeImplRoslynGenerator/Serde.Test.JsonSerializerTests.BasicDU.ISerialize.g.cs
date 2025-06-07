
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        sealed partial class _SerObj : Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.BasicDU.s_serdeInfo;

            void ISerialize<Serde.Test.JsonSerializerTests.BasicDU>.Serialize(Serde.Test.JsonSerializerTests.BasicDU value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_serdeInfo);
                switch (value)
                {
                    case Serde.Test.JsonSerializerTests.BasicDU.A c:
                        _l_type.WriteValue<Serde.Test.JsonSerializerTests.BasicDU.A, _m_AProxy>(_l_serdeInfo, 0, c);
                        break;
                    case Serde.Test.JsonSerializerTests.BasicDU.B c:
                        _l_type.WriteValue<Serde.Test.JsonSerializerTests.BasicDU.B, _m_BProxy>(_l_serdeInfo, 1, c);
                        break;

                }
                _l_type.End(_l_serdeInfo);
            }
        }
    }
}
