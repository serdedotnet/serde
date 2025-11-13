
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class Int128Tests
{
    partial record UInt128Wrap
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.Int128Tests.UInt128Wrap>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.Int128Tests.UInt128Wrap.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.Int128Tests.UInt128Wrap>.Serialize(Serde.Test.Int128Tests.UInt128Wrap value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteU128(_l_info, 0, value.Value);
                _l_type.End(_l_info);
            }
            Serde.Test.Int128Tests.UInt128Wrap Serde.IDeserialize<Serde.Test.Int128Tests.UInt128Wrap>.Deserialize(IDeserializer deserializer)
            {
                System.UInt128 _l_value = default!;

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
                            _l_value = typeDeserialize.ReadU128(_l_serdeInfo, _l_index_);
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
                var newType = new Serde.Test.Int128Tests.UInt128Wrap(_l_value) {
                };

                return newType;
            }
        }
    }
}
