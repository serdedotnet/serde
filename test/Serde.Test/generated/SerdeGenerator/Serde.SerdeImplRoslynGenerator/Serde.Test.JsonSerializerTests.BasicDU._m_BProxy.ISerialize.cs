
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_BProxy : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.B>
        {
            static ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B> ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.B>.SerializeInstance
                => _SerObj.Instance;

            sealed partial class _SerObj :Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B>
            {
                void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B>.Serialize(Serde.Test.JsonSerializerTests.BasicDU.B value, global::Serde.ISerializer serializer)
                {
                    var _l_info = global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>();
                    var _l_type = serializer.WriteType(_l_info);
                    _l_type.WriteString(_l_info, 0, value.Y);
                    _l_type.End(_l_info);
                }
                public static readonly _SerObj Instance = new();
                private _SerObj() { }

            }
        }
    }
}
