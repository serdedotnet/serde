
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
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,0,value.X);
                    type.End();
                }
                public static readonly _m_AProxySerializeProxy Instance = new();
                private _m_AProxySerializeProxy() { }

            }
        }
    }
}
