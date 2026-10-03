
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record DUWithInitializer
    {
        partial class _m_AProxy
        {
            sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DUWithInitializer.A>
            {
                global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.DUWithInitializer._m_AProxy.s_serdeInfo;

                Serde.Test.JsonDeserializeTests.DUWithInitializer.A Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DUWithInitializer.A>.Deserialize(IDeserializer deserializer)
                {
                    string _l_name = default!;
                    bool _l_flag = true;

                    byte _r_assignedValid = 0;

                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    while (true)
                    {
                        var (_l_index_, _) = typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                        if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                        {
                            break;
                        }

                        switch (_l_index_)
                        {
                            case 0:
                                Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 0, _l_serdeInfo);
                                _l_name = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 1:
                                Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                                _l_flag = typeDeserialize.ReadBool(_l_serdeInfo, _l_index_);
                                _r_assignedValid |= ((byte)1) << 1;
                                break;
                            case Serde.ITypeDeserializer.IndexNotFound:
                                typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }
                    typeDeserialize.End(_l_serdeInfo);
                    if ((_r_assignedValid & 0b1) != 0b1)
                    {
                        throw Serde.DeserializeException.UnassignedMember(_r_assignedValid, 0b1, _l_serdeInfo);
                    }
                    var newType = new Serde.Test.JsonDeserializeTests.DUWithInitializer.A() {
                        Name = _l_name,
                        Flag = _l_flag,
                    };

                    return newType;
                }
            }
        }
    }
}
