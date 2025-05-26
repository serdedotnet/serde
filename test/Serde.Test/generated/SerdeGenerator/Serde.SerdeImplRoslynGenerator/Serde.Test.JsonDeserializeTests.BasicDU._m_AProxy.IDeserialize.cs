
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_AProxy
        {
            sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.A>
            {
                global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.BasicDU._m_AProxy.s_serdeInfo;

                Serde.Test.JsonDeserializeTests.BasicDU.A Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.A>.Deserialize(IDeserializer deserializer)
                {
                    int _l_x = default!;

                    byte _r_assignedValid = 0;

                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case 0:
                                _l_x = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case Serde.ITypeDeserializer.IndexNotFound:
                                typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }
                    if ((_r_assignedValid & 0b1) != 0b1)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }
                    var newType = new Serde.Test.JsonDeserializeTests.BasicDU.A(_l_x) {
                    };

                    return newType;
                }
            }
        }
    }
}
