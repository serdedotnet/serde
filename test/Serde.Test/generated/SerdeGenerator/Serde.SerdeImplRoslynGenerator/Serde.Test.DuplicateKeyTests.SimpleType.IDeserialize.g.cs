
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct SimpleType
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.DuplicateKeyTests.SimpleType>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.DuplicateKeyTests.SimpleType.s_serdeInfo;

            Serde.Test.DuplicateKeyTests.SimpleType Serde.IDeserialize<Serde.Test.DuplicateKeyTests.SimpleType>.Deserialize(IDeserializer deserializer)
            {
                string _l_name = default!;
                int _l_value = default!;

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
                            _l_value = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b11) != 0b11)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.DuplicateKeyTests.SimpleType() {
                    Name = _l_name,
                    Value = _l_value,
                };

                return newType;
            }
        }
    }
}
