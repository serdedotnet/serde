
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPoco
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithPoco>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Json.Test.InvalidJsonTests.ClassWithPoco.s_serdeInfo;

            async global::System.Threading.Tasks.Task<Serde.Json.Test.InvalidJsonTests.ClassWithPoco> Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithPoco>.Deserialize(IDeserializer deserializer)
            {
                Serde.Json.Test.Poco _l_obj = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                while (true)
                {
                    var (_l_index_, _) = await typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                    if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                    {
                        break;
                    }

                    switch (_l_index_)
                    {
                        case 0:
                            _l_obj = await typeDeserialize.ReadValue<Serde.Json.Test.Poco, Serde.Json.Test.Poco>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Json.Test.InvalidJsonTests.ClassWithPoco() {
                    Obj = _l_obj,
                };

                return newType;
            }
        }
    }
}
