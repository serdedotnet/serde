
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class MemberOrdinalTests
{
    partial record Reordered
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.MemberOrdinalTests.Reordered>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.MemberOrdinalTests.Reordered.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.MemberOrdinalTests.Reordered>.Serialize(Serde.Test.MemberOrdinalTests.Reordered value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.B);
                _l_type.WriteI32(_l_info, 1, value.C);
                _l_type.WriteI32(_l_info, 2, value.A);
                _l_type.End(_l_info);
            }
            Serde.Test.MemberOrdinalTests.Reordered Serde.IDeserialize<Serde.Test.MemberOrdinalTests.Reordered>.Deserialize(IDeserializer deserializer)
            {
                int _l_b = default!;
                int _l_c = default!;
                int _l_a = default!;

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
                            _l_b = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_c = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case 2:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                            _l_a = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 2;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b111) != 0b111)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.MemberOrdinalTests.Reordered() {
                    B = _l_b,
                    C = _l_c,
                    A = _l_a,
                };

                return newType;
            }
        }
    }
}
