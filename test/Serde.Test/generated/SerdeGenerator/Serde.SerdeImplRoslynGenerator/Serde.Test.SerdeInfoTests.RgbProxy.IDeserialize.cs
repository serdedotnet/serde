
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SerdeInfoTests
    {
        partial record RgbProxy : Serde.IDeserialize<Serde.Test.SerdeInfoTests.Rgb>
        {
            static Serde.Test.SerdeInfoTests.Rgb Serde.IDeserialize<Serde.Test.SerdeInfoTests.Rgb>.Deserialize(IDeserializer deserializer)
            {
                byte _l_r = default !;
                byte _l_g = default !;
                byte _l_b = default !;
                byte _r_assignedValid = 0;
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<RgbProxy>();
                var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_r = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_g = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case 2:
                            _l_b = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 2;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b111) != 0b111)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Serde.Test.SerdeInfoTests.Rgb()
                {
                    R = _l_r,
                    G = _l_g,
                    B = _l_b,
                };
                return newType;
            }
        }
    }
}