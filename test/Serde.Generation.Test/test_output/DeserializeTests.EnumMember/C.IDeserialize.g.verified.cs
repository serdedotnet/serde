//HintName: C.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _DeObj : Serde.IDeserialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        async global::System.Threading.Tasks.ValueTask<C> Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            ColorInt _l_colorint = default!;
            ColorByte _l_colorbyte = default!;
            ColorLong _l_colorlong = default!;
            ColorULong _l_colorulong = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_colorint = await typeDeserialize.ReadBoxedValue<ColorInt, ColorIntProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_colorbyte = await typeDeserialize.ReadBoxedValue<ColorByte, ColorByteProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_colorlong = await typeDeserialize.ReadBoxedValue<ColorLong, ColorLongProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        _l_colorulong = await typeDeserialize.ReadBoxedValue<ColorULong, ColorULongProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1111) != 0b1111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new C() {
                ColorInt = _l_colorint,
                ColorByte = _l_colorbyte,
                ColorLong = _l_colorlong,
                ColorULong = _l_colorulong,
            };

            return newType;
        }
    }
}
