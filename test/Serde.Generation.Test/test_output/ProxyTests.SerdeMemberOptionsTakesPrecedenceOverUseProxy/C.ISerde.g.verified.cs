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
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteBoxedValue<System.Runtime.InteropServices.ComTypes.BIND_OPTS, Proxy2>(_l_info, 0, value.S);
            _l_type.End(_l_info);
        }
        C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            System.Runtime.InteropServices.ComTypes.BIND_OPTS _l_s = default!;

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
                        _l_s = typeDeserialize.ReadBoxedValue<System.Runtime.InteropServices.ComTypes.BIND_OPTS, Proxy2>(_l_serdeInfo, _l_index_);
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
            var newType = new C() {
                S = _l_s,
            };

            return newType;
        }
    }
}
