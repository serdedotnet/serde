
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerializeNullTests
{
    partial class TypeOption
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.SerializeNullTests.TypeOption>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SerializeNullTests.TypeOption.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.SerializeNullTests.TypeOption>.Serialize(Serde.Test.SerializeNullTests.TypeOption value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info, 1);
                _l_type.WriteValue<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(_l_info, 0, value.Value);
                _l_type.End(_l_info);
            }
            Serde.Test.SerializeNullTests.TypeOption Serde.IDeserialize<Serde.Test.SerializeNullTests.TypeOption>.Deserialize(IDeserializer deserializer)
            {
                string? _l_value = default!;

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
                            _l_value = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
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
                    throw Serde.DeserializeException.UnassignedMember(_r_assignedValid, 0b0, _l_serdeInfo);
                }
                var newType = new Serde.Test.SerializeNullTests.TypeOption() {
                    Value = _l_value,
                };

                return newType;
            }
        }
    }
}
