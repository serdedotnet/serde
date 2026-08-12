//HintName: C.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _SerdeObj : global::Serde.ISerde<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        void global::Serde.ISerialize<C>.Serialize(C value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_fieldCount = 2;
            if (value.OptP is null) _l_fieldCount--;
            var _l_type = serializer.WriteType(_l_info, _l_fieldCount);
            _l_type.WriteValue<Point, PointProxy>(_l_info, 0, value.P);
            _l_type.WriteValueIfNotNull<Point, Serde.NullableRefProxy.Ser<Point, PointProxy>>(_l_info, 1, value.OptP);
            _l_type.End(_l_info);
        }
        C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            Point _l_p = default!;
            Point? _l_optp = null;

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
                        _l_p = typeDeserialize.ReadValue<Point, PointProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_optp = typeDeserialize.ReadValue<Point?, Serde.NullableRefProxy.De<Point, PointProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            typeDeserialize.End(_l_serdeInfo);
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new C() {
                P = _l_p,
                OptP = _l_optp,
            };

            return newType;
        }
    }
}
