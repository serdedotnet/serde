//HintName: S.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize(IDeserializer deserializer)
    {
        ColorEnum _l_e = default !;
        byte _r_assignedValid = 0b0;
        var _l_typeInfo = SSerdeTypeInfo.TypeInfo;
        var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_e = typeDeserialize.ReadValue<ColorEnum, global::ColorEnumWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if (_r_assignedValid != 0b1)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new S()
        {
            E = _l_e,
        };
        return newType;
    }
}