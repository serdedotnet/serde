
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_BProxy : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU.B>
        {
            static IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.B> IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU.B>.DeserializeInstance
                => _DeObj.Instance;

            sealed partial class _DeObj :Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.B>
            {
                Serde.Test.JsonDeserializeTests.BasicDU.B Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.B>.Deserialize(IDeserializer deserializer)
                {
                    string _l_y = default!;

                    byte _r_assignedValid = 0;

                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_BProxy>();
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case 0:
                                _l_y = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
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
                    var newType = new Serde.Test.JsonDeserializeTests.BasicDU.B(_l_y) {
                    };

                    return newType;
                }
                public static readonly _DeObj Instance = new();
                private _DeObj() { }

            }
        }
    }
}
