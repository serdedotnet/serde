
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class RoundtripTests
{
    partial record ProxiedContainer
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.RoundtripTests.ProxiedContainer>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.RoundtripTests.ProxiedContainer.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.RoundtripTests.ProxiedContainer>.Serialize(Serde.Test.RoundtripTests.ProxiedContainer value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_fieldCount = 2;
                if (value.OptionalPoint is null) _l_fieldCount--;
                var _l_type = serializer.WriteType(_l_info, _l_fieldCount);
                _l_type.WriteValue<Serde.Test.RoundtripTests.ForeignPoint, Serde.Test.RoundtripTests.ForeignPointProxy>(_l_info, 0, value.Point);
                _l_type.WriteValueIfNotNull<Serde.Test.RoundtripTests.ForeignPoint, Serde.NullableRefProxy.Ser<Serde.Test.RoundtripTests.ForeignPoint, Serde.Test.RoundtripTests.ForeignPointProxy>>(_l_info, 1, value.OptionalPoint);
                _l_type.End(_l_info);
            }
            Serde.Test.RoundtripTests.ProxiedContainer Serde.IDeserialize<Serde.Test.RoundtripTests.ProxiedContainer>.Deserialize(IDeserializer deserializer)
            {
                Serde.Test.RoundtripTests.ForeignPoint _l_point = default!;
                Serde.Test.RoundtripTests.ForeignPoint? _l_optionalpoint = default!;

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
                            _l_point = typeDeserialize.ReadValue<Serde.Test.RoundtripTests.ForeignPoint, Serde.Test.RoundtripTests.ForeignPointProxy>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_optionalpoint = typeDeserialize.ReadValue<Serde.Test.RoundtripTests.ForeignPoint?, Serde.NullableRefProxy.De<Serde.Test.RoundtripTests.ForeignPoint, Serde.Test.RoundtripTests.ForeignPointProxy>>(_l_serdeInfo, _l_index_);
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
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.RoundtripTests.ProxiedContainer() {
                    Point = _l_point,
                    OptionalPoint = _l_optionalpoint,
                };

                return newType;
            }
        }
    }
}
