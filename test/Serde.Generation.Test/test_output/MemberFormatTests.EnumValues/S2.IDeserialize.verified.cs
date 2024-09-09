//HintName: S2.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S2 : Serde.IDeserialize<S2>
{
    static S2 Serde.IDeserialize<S2>.Deserialize(IDeserializer deserializer)
    {
        ColorEnum _l_e = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S2>();
        var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_e = typeDeserialize.ReadValue<ColorEnum, ColorEnumWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    typeDeserialize.SkipValue();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b1) != 0b1)
        {
            throw Serde.DeserializeException.UnassignedMember();
        }

        var newType = new S2()
        {
            E = _l_e,
        };
        return newType;
    }
}