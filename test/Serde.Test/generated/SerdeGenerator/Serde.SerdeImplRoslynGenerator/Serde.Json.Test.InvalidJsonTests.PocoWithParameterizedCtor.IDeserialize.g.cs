
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class PocoWithParameterizedCtor
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor.s_serdeInfo;

            Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor>.Deserialize(IDeserializer deserializer)
            {
                int _l_obj = default!;

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
                            _l_obj = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
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
                var newType = new Serde.Json.Test.InvalidJsonTests.PocoWithParameterizedCtor() {
                    Obj = _l_obj,
                };

                return newType;
            }
        }
    }
}
