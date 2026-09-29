
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record OptionalFields
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.SerdeInfoTests.OptionalFields>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SerdeInfoTests.OptionalFields.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.SerdeInfoTests.OptionalFields>.Serialize(Serde.Test.SerdeInfoTests.OptionalFields value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_fieldCount = 14;
                if (value.NullableReference is null) _l_fieldCount--;
                if (value.NullableValue is null) _l_fieldCount--;
                if (value.RequiredNullableReference is null) _l_fieldCount--;
                if (value.RequiredNullableValue is null) _l_fieldCount--;
                var _l_type = serializer.WriteType(_l_info, _l_fieldCount);
                _l_type.WriteString(_l_info, 0, value.RequiredReference);
                _l_type.WriteI32(_l_info, 1, value.RequiredValue);
                _l_type.WriteStringIfNotNull(_l_info, 2, value.NullableReference);
                _l_type.WriteValueIfNotNull<int, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 3, value.NullableValue);
                _l_type.WriteStringIfNotNull(_l_info, 4, value.RequiredNullableReference);
                _l_type.WriteValueIfNotNull<int, Serde.NullableProxy.Ser<int, global::Serde.I32Proxy>>(_l_info, 5, value.RequiredNullableValue);
                _l_type.WriteString(_l_info, 6, value.OptionalReference);
                _l_type.WriteI32(_l_info, 7, value.OptionalValue);
                _l_type.WriteString(_l_info, 8, value.InitializedReference);
                _l_type.WriteI32(_l_info, 9, value.InitializedValue);
                _l_type.WriteString(_l_info, 10, value.StaticInitializer);
                _l_type.WriteString(_l_info, 11, value.UnsupportedInitializer);
                _l_type.WriteI32(_l_info, 12, value.RequiredInitializer);
                _l_type.WriteI32(_l_info, 13, value.Skipped);
                _l_type.End(_l_info);
            }
            Serde.Test.SerdeInfoTests.OptionalFields Serde.IDeserialize<Serde.Test.SerdeInfoTests.OptionalFields>.Deserialize(IDeserializer deserializer)
            {
                string _l_requiredreference = default!;
                int _l_requiredvalue = default!;
                string? _l_nullablereference = default!;
                int? _l_nullablevalue = default!;
                string? _l_requirednullablereference = default!;
                int? _l_requirednullablevalue = default!;
                string _l_optionalreference = default!;
                int _l_optionalvalue = default!;
                string _l_initializedreference = "default";
                int _l_initializedvalue = 42;
                string _l_staticinitializer = Serde.Test.SerdeInfoTests.OptionalFields.DefaultText;
                string _l_unsupportedinitializer = default!;
                int _l_requiredinitializer = 42;

                ushort _r_assignedValid = 0;

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
                            _l_requiredreference = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_requiredvalue = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 1;
                            break;
                        case 2:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                            _l_nullablereference = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 2;
                            break;
                        case 3:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
                            _l_nullablevalue = typeDeserialize.ReadValue<int?, Serde.NullableProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 3;
                            break;
                        case 4:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 4, _l_serdeInfo);
                            _l_requirednullablereference = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 4;
                            break;
                        case 5:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 5, _l_serdeInfo);
                            _l_requirednullablevalue = typeDeserialize.ReadValue<int?, Serde.NullableProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 5;
                            break;
                        case 6:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 6, _l_serdeInfo);
                            _l_optionalreference = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 6;
                            break;
                        case 7:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 7, _l_serdeInfo);
                            _l_optionalvalue = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 7;
                            break;
                        case 8:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 8, _l_serdeInfo);
                            _l_initializedreference = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 8;
                            break;
                        case 9:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 9, _l_serdeInfo);
                            _l_initializedvalue = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 9;
                            break;
                        case 10:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 10, _l_serdeInfo);
                            _l_staticinitializer = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 10;
                            break;
                        case 11:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 11, _l_serdeInfo);
                            _l_unsupportedinitializer = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 11;
                            break;
                        case 12:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 12, _l_serdeInfo);
                            _l_requiredinitializer = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((ushort)1) << 12;
                            break;
                        case 13:
                        case Serde.ITypeDeserializer.IndexNotFound:
                            typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                typeDeserialize.End(_l_serdeInfo);
                if ((_r_assignedValid & 0b1100000110011) != 0b1100000110011)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.SerdeInfoTests.OptionalFields() {
                    RequiredReference = _l_requiredreference,
                    RequiredValue = _l_requiredvalue,
                    NullableReference = _l_nullablereference,
                    NullableValue = _l_nullablevalue,
                    RequiredNullableReference = _l_requirednullablereference,
                    RequiredNullableValue = _l_requirednullablevalue,
                    OptionalReference = _l_optionalreference,
                    OptionalValue = _l_optionalvalue,
                    InitializedReference = _l_initializedreference,
                    InitializedValue = _l_initializedvalue,
                    StaticInitializer = _l_staticinitializer,
                    UnsupportedInitializer = _l_unsupportedinitializer,
                    RequiredInitializer = _l_requiredinitializer,
                };

                return newType;
            }
        }
    }
}
