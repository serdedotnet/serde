//HintName: Rgb.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial struct Rgb
{
    sealed partial class _DeObj : Serde.IDeserialize<Rgb>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Rgb.s_serdeInfo;

        async global::System.Threading.Tasks.Task<Rgb> Serde.IDeserialize<Rgb>.Deserialize(IDeserializer deserializer)
        {
            byte _l_red = default!;
            byte _l_blue = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            while (true)
            {
                var (_l_index_, _) = await typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                {
                    break;
                }

                switch (_l_index_)
                {
                    case 0:
                        _l_red = await typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_blue = await typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b11) != 0b11)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Rgb() {
                Red = _l_red,
                Blue = _l_blue,
            };

            return newType;
        }
    }
}
