//HintName: C.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _DeObj : Serde.IDeserialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            int _l_z = default!;
            int _l_y = default!;
            int _l_x = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_z = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_y = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_x = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b111) != 0b111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new C() {
                Z = _l_z,
                Y = _l_y,
                X = _l_x,
            };

            return newType;
        }
    }
}
