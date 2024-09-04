
#nullable enable
using System;
using Serde;

namespace Serde.Json.Test
{
    partial class InvalidJsonTests
    {
        partial class SkipDoubleCommaClass : Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.SkipDoubleCommaClass>
        {
            static Serde.Json.Test.InvalidJsonTests.SkipDoubleCommaClass Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.SkipDoubleCommaClass>.Deserialize(IDeserializer deserializer)
            {
                int _l_c = default !;
                byte _r_assignedValid = 0;
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<SkipDoubleCommaClass>();
                var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_c = typeDeserialize.ReadValue<int, global::Serde.Int32Wrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }

                var newType = new Serde.Json.Test.InvalidJsonTests.SkipDoubleCommaClass()
                {
                    C = _l_c,
                };
                return newType;
            }
        }
    }
}