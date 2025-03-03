
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_AProxy : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.A>
        {
            static ISerialize<Serde.Test.JsonSerializerTests.BasicDU.A> ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.A>.SerializeInstance
                => _m_AProxySerializeProxy.Instance;

            sealed partial class _m_AProxySerializeProxy :Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.A>
            {
                void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.A>.Serialize(Serde.Test.JsonSerializerTests.BasicDU.A value, global::Serde.ISerializer serializer)
                {
                    var _l_info = global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>();
                    var _l_type = serializer.WriteType(_l_info);
                    _l_type.WriteI32(_l_info, 0, value.X);
                    _l_type.End(_l_info);
                }
                public static readonly _m_AProxySerializeProxy Instance = new();
                private _m_AProxySerializeProxy() { }

            }
        }
    }
}
