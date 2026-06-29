
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class NullableValueTypeTests
{
    partial record TwoFields
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.NullableValueTypeTests.TwoFields>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.NullableValueTypeTests.TwoFields.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.NullableValueTypeTests.TwoFields>.Serialize(Serde.Test.NullableValueTypeTests.TwoFields value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValueIfNotNull<int, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 0, value.First);
                _l_type.WriteValueIfNotNull<int, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 1, value.Second);
                _l_type.End(_l_info);
            }
            Serde.Test.NullableValueTypeTests.TwoFields Serde.IDeserialize<Serde.Test.NullableValueTypeTests.TwoFields>.Deserialize(IDeserializer deserializer)
            {
                int? _l_first = default!;
                int? _l_second = default!;

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
                            _l_first = typeDeserialize.ReadValue<int?, Serde.NullableProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_second = typeDeserialize.ReadValue<int?, Serde.NullableProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
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
                if ((_r_assignedValid & 0b0) != 0b0)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.NullableValueTypeTests.TwoFields() {
                    First = _l_first,
                    Second = _l_second,
                };

                return newType;
            }
        }
    }
}
