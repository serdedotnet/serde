
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_BProxy : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.B>
        {
            static IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.B> IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.B>.DeserializeInstance
                => _m_BProxyDeserializeProxy.Instance;

            sealed partial class _m_BProxyDeserializeProxy :Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.B>
            {
                Serde.Test.SerdeInfoTests.UnionBase.B Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.B>.Deserialize(IDeserializer deserializer)
                {

                    byte _r_assignedValid = 0;

                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>();
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case Serde.IDeserializeType.IndexNotFound:
                                typeDeserialize.SkipValue();
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }
                    if ((_r_assignedValid & 0b0) != 0b0)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }
                    var newType = new Serde.Test.SerdeInfoTests.UnionBase.B() {
                    };

                    return newType;
                }
                public static readonly _m_BProxyDeserializeProxy Instance = new();
                private _m_BProxyDeserializeProxy() { }

            }
        }
    }
}
