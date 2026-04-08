
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class Vector2Proxy
{
    sealed partial class _SerdeObj : global::Serde.ISerde<System.Numerics.Vector2>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Json.Test.Vector2Proxy.s_serdeInfo;

        void global::Serde.ISerialize<System.Numerics.Vector2>.Serialize(System.Numerics.Vector2 value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteF32(_l_info, 0, value.X);
            _l_type.WriteF32(_l_info, 1, value.Y);
            _l_type.End(_l_info);
        }
        System.Numerics.Vector2 Serde.IDeserialize<System.Numerics.Vector2>.Deserialize(IDeserializer deserializer)
        {
            float _l_x = default!;
            float _l_y = default!;

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
                        _l_x = typeDeserialize.ReadF32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_y = typeDeserialize.ReadF32(_l_serdeInfo, _l_index_);
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
            var newType = new System.Numerics.Vector2() {
                X = _l_x,
                Y = _l_y,
            };

            return newType;
        }
    }
}
