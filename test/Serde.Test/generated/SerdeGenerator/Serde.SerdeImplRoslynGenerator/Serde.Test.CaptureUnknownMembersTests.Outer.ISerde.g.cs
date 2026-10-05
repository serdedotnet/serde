
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial record Outer
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.CaptureUnknownMembersTests.Outer>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.CaptureUnknownMembersTests.Outer.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.CaptureUnknownMembersTests.Outer>.Serialize(Serde.Test.CaptureUnknownMembersTests.Outer value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info, 2);
                _l_type.WriteString(_l_info, 0, value.Id);
                _l_type.WriteValue<Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown, Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown>(_l_info, 1, value.Inner);
                _l_type.End(_l_info);
            }
            Serde.Test.CaptureUnknownMembersTests.Outer Serde.IDeserialize<Serde.Test.CaptureUnknownMembersTests.Outer>.Deserialize(IDeserializer deserializer)
            {
                string _l_id = default!;
                Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown _l_inner = default!;

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
                            _l_id = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_inner = typeDeserialize.ReadValue<Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown, Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown>(_l_serdeInfo, _l_index_);
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
                if ((_r_assignedValid & 0b11) != 0b11)
                {
                    throw Serde.DeserializeException.UnassignedMember(_r_assignedValid, 0b11, _l_serdeInfo);
                }
                var newType = new Serde.Test.CaptureUnknownMembersTests.Outer(_l_id, _l_inner) {
                };

                return newType;
            }
        }
    }
}
