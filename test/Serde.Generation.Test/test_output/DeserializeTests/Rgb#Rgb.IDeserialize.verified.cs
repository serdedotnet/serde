//HintName: Rgb.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize(IDeserializer deserializer)
    {
        byte _l_red = default !;
        byte _l_green = default !;
        byte _l_blue = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Rgb>();
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_red = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_green = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case 2:
                    _l_blue = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 2;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    typeDeserialize.SkipValue();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b111) != 0b111)
        {
            throw Serde.DeserializeException.UnassignedMember();
        }

        var newType = new Rgb()
        {
            Red = _l_red,
            Green = _l_green,
            Blue = _l_blue,
        };
        return newType;
    }
}