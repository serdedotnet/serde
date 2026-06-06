
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class RoundtripTests
{
    partial struct ForeignPointProxy
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.RoundtripTests.ForeignPoint>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.RoundtripTests.ForeignPointProxy.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.RoundtripTests.ForeignPoint>.Serialize(Serde.Test.RoundtripTests.ForeignPoint value, global::Serde.ISerializer serializer)
            {
                var _l_self = (Serde.Test.RoundtripTests.ForeignPointProxy)value;
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, _l_self.X);
                _l_type.WriteI32(_l_info, 1, _l_self.Y);
                _l_type.End(_l_info);
            }
            Serde.Test.RoundtripTests.ForeignPoint Serde.IDeserialize<Serde.Test.RoundtripTests.ForeignPoint>.Deserialize(IDeserializer deserializer)
            {
                int _l_x = default!;
                int _l_y = default!;

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
                            _l_x = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_y = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
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
                var newType = new Serde.Test.RoundtripTests.ForeignPointProxy() {
                    X = _l_x,
                    Y = _l_y,
                };

                return (Serde.Test.RoundtripTests.ForeignPoint)newType;
            }
        }
    }
}
