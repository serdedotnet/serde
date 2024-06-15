//HintName: PointWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct PointWrap : Serde.IDeserialize<Point>
{
    static Point Serde.IDeserialize<Point>.Deserialize(IDeserializer deserializer)
    {
        int _l_x = default !;
        int _l_y = default !;
        byte _r_assignedValid = 0;
        var _l_typeInfo = PointSerdeTypeInfo.TypeInfo;
        var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_x = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_y = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b11) != 0b11)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new Point()
        {
            X = _l_x,
            Y = _l_y,
        };
        return newType;
    }
}