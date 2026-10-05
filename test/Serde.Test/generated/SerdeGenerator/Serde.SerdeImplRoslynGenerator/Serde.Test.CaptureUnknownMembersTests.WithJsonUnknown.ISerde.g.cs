
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class CaptureUnknownMembersTests
{
    partial class WithJsonUnknown
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown>.Serialize(Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_fieldCount = 2;
                if (value.Age is null) _l_fieldCount--;
                _l_fieldCount += value.Unknown?.Count ?? 0;
                var _l_type = serializer.WriteType(_l_info, _l_fieldCount);
                _l_type.WriteString(_l_info, 0, value.Name);
                _l_type.WriteValueIfNotNull<int, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 1, value.Age);
                if (value.Unknown is { } _l_unknown)
                {
                    global::Serde.UnknownMembers.Serialize<Serde.Json.JsonValue, Serde.Json.JsonValue>(_l_unknown, _l_type, _l_info);
                }
                _l_type.End(_l_info);
            }
            Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown Serde.IDeserialize<Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown>.Deserialize(IDeserializer deserializer)
            {
                string _l_name = default!;
                int? _l_age = default!;
                System.Collections.Generic.Dictionary<string, Serde.Json.JsonValue>? _l_unknown = null;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                while (true)
                {
                    var (_l_index_, _l_errorName) = typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
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
                            _l_age = typeDeserialize.ReadValue<int?, Serde.NullableProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            if (_l_errorName is null)
                            {
                                typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                                break;
                            }
                            var _l_unknownValue = typeDeserialize.ReadValue<Serde.Json.JsonValue, Serde.Json.JsonValue>(_l_serdeInfo, _l_index_);
                            _l_unknown ??= new();
                            if (!_l_unknown.TryAdd(_l_errorName, _l_unknownValue))
                            {
                                throw Serde.DeserializeException.DuplicateKey(_l_errorName, _l_serdeInfo);
                            }
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
                var newType = new Serde.Test.CaptureUnknownMembersTests.WithJsonUnknown() {
                    Name = _l_name,
                    Age = _l_age,
                    Unknown = _l_unknown,
                };

                return newType;
            }
        }
    }
}
