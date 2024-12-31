
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial record BasicDU
        {
            partial class _m_BProxy : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.B>
            {
                static ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B> ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.B>.SerializeInstance => _m_BProxySerializeProxy.Instance;

                sealed class _m_BProxySerializeProxy : Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B>
                {
                    void ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B>.Serialize(Serde.Test.JsonSerializerTests.BasicDU.B value, ISerializer serializer)
                    {
                        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>();
                        var type = serializer.SerializeType(_l_serdeInfo);
                        type.SerializeField<string, global::Serde.StringProxy>(_l_serdeInfo, 0, value.Y);
                        type.End();
                    }

                    public static readonly _m_BProxySerializeProxy Instance = new();
                    private _m_BProxySerializeProxy()
                    {
                    }
                }
            }
        }
    }
}