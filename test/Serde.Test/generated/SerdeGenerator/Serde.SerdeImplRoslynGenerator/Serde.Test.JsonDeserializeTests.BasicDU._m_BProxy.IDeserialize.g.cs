
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_BProxy
        {
            sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.B>
            {
                global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.BasicDU._m_BProxy.s_serdeInfo;

                async global::System.Threading.Tasks.Task<Serde.Test.JsonDeserializeTests.BasicDU.B> Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.B>.Deserialize(IDeserializer deserializer)
                {
                    string _l_y = default!;

                    byte _r_assignedValid = 0;

                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    while (true)
                    {
                        var (_l_index_, _) = await typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                        if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                        {
                            break;
                        }

                        switch (_l_index_)
                        {
                            case 0:
                                _l_y = await typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case Serde.ITypeDeserializer.IndexNotFound:
                                await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }
                    if ((_r_assignedValid & 0b1) != 0b1)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }
                    var newType = new Serde.Test.JsonDeserializeTests.BasicDU.B(_l_y) {
                    };

                    return newType;
                }
            }
        }
    }
}
