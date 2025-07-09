
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record RgbProxy
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.SerdeInfoTests.Rgb>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SerdeInfoTests.RgbProxy.s_serdeInfo;

            async global::System.Threading.Tasks.Task<Serde.Test.SerdeInfoTests.Rgb> Serde.IDeserialize<Serde.Test.SerdeInfoTests.Rgb>.Deserialize(IDeserializer deserializer)
            {
                byte _l_r = default!;
                byte _l_g = default!;
                byte _l_b = default!;

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
                            _l_r = await typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_g = await typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case 2:
                            _l_b = await typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 2;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b111) != 0b111)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.SerdeInfoTests.Rgb() {
                    R = _l_r,
                    G = _l_g,
                    B = _l_b,
                };

                return newType;
            }
        }
    }
}
