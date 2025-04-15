//HintName: PointWrap.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial struct PointWrap : Serde.IDeserializeProvider<Point>
{
    static IDeserialize<Point> IDeserializeProvider<Point>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<Point>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => PointWrap.s_serdeInfo;

        Point Serde.IDeserialize<Point>.Deserialize(IDeserializer deserializer)
        {
            int _l_x = default!;
            int _l_y = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_x = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
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
            var newType = new Point() {
                X = _l_x,
                Y = _l_y,
            };

            return newType;
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
