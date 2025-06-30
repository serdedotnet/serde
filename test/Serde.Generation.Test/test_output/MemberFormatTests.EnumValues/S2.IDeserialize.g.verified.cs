//HintName: S2.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial struct S2
{
    sealed partial class _DeObj : Serde.IDeserialize<S2>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => S2.s_serdeInfo;

        async global::System.Threading.Tasks.ValueTask<S2> Serde.IDeserialize<S2>.Deserialize(IDeserializer deserializer)
        {
            ColorEnum _l_e = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_e = await typeDeserialize.ReadBoxedValue<ColorEnum, ColorEnumProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new S2() {
                E = _l_e,
            };

            return newType;
        }
    }
}
