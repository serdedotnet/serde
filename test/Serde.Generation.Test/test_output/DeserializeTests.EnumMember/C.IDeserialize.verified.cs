//HintName: C.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
    {
        ColorInt _l_colorint = default !;
        ColorByte _l_colorbyte = default !;
        ColorLong _l_colorlong = default !;
        ColorULong _l_colorulong = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
        var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_colorint = typeDeserialize.ReadValue<ColorInt, ColorIntWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_colorbyte = typeDeserialize.ReadValue<ColorByte, ColorByteWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case 2:
                    _l_colorlong = typeDeserialize.ReadValue<ColorLong, ColorLongWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 2;
                    break;
                case 3:
                    _l_colorulong = typeDeserialize.ReadValue<ColorULong, ColorULongWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 3;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    typeDeserialize.SkipValue();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b1111) != 0b1111)
        {
            throw Serde.DeserializeException.UnassignedMember();
        }

        var newType = new C()
        {
            ColorInt = _l_colorint,
            ColorByte = _l_colorbyte,
            ColorLong = _l_colorlong,
            ColorULong = _l_colorulong,
        };
        return newType;
    }
}