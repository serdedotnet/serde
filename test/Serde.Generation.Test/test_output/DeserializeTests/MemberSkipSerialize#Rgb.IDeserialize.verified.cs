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
        var _l_typeInfo = RgbSerdeTypeInfo.TypeInfo;
        var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_red = typeDeserialize.ReadValue<byte, ByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_green = typeDeserialize.ReadValue<byte, ByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case 2:
                    _l_blue = typeDeserialize.ReadValue<byte, ByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 2;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b111) != 0b111)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
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