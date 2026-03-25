
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class Test
{
    sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Json.Test.Test>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Json.Test.Test.s_serdeInfo;

        void global::Serde.ISerialize<Serde.Json.Test.Test>.Serialize(Serde.Json.Test.Test value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValue<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>(_l_info, 0, value.v2);
            _l_type.WriteValue<System.Numerics.Vector2[][], Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>(_l_info, 1, value.vertices);
            _l_type.WriteValue<System.Numerics.Vector2[][], Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy2>>>(_l_info, 2, value.weights);
            _l_type.WriteValue<System.Collections.Generic.Dictionary<System.Numerics.Vector3, System.Numerics.Vector2[][]>, Serde.DictProxy.Ser<System.Numerics.Vector3, System.Numerics.Vector2[][], Serde.Json.Test.Vector3Proxy, Serde.ArrayProxy.Ser<System.Numerics.Vector2[], Serde.ArrayProxy.Ser<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>>(_l_info, 3, value.points);
            _l_type.End(_l_info);
        }
        Serde.Json.Test.Test Serde.IDeserialize<Serde.Json.Test.Test>.Deserialize(IDeserializer deserializer)
        {
            System.Numerics.Vector2 _l_v2 = default!;
            System.Numerics.Vector2[][] _l_vertices = default!;
            System.Numerics.Vector2[][] _l_weights = default!;
            System.Collections.Generic.Dictionary<System.Numerics.Vector3, System.Numerics.Vector2[][]> _l_points = default!;

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
                        _l_v2 = typeDeserialize.ReadBoxedValue<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_vertices = typeDeserialize.ReadValue<System.Numerics.Vector2[][], Serde.ArrayProxy.De<System.Numerics.Vector2[], Serde.ArrayProxy.De<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                        _l_weights = typeDeserialize.ReadValue<System.Numerics.Vector2[][], Serde.ArrayProxy.De<System.Numerics.Vector2[], Serde.ArrayProxy.De<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy>>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
                        _l_points = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<System.Numerics.Vector3, System.Numerics.Vector2[][]>, Serde.DictProxy.De<System.Numerics.Vector3, System.Numerics.Vector2[][], Serde.Json.Test.Vector3Proxy, Serde.ArrayProxy.De<System.Numerics.Vector2[], Serde.ArrayProxy.De<System.Numerics.Vector2, Serde.Json.Test.Vector2Proxy2>>>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1111) != 0b1111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Serde.Json.Test.Test() {
                v2 = _l_v2,
                vertices = _l_vertices,
                weights = _l_weights,
                points = _l_points,
            };

            return newType;
        }
    }
}
