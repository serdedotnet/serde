
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial struct ExtraMembers
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ExtraMembers>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.ExtraMembers.s_serdeInfo;

            Serde.Test.JsonDeserializeTests.ExtraMembers Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ExtraMembers>.Deserialize(IDeserializer deserializer)
            {
                int _l_b = default!;

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
                            _l_b = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
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
                var newType = new Serde.Test.JsonDeserializeTests.ExtraMembers() {
                    b = _l_b,
                };

                return newType;
            }
        }
    }
}
