//HintName: SetToNull.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct SetToNull : Serde.IDeserialize<SetToNull>
{
    static SetToNull Serde.IDeserialize<SetToNull>.Deserialize(IDeserializer deserializer)
    {
        string _l_present = default !;
        string? _l_missing = default !;
        string? _l_throwmissing = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<SetToNull>();
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_present = typeDeserialize.ReadValue<string, global::Serde.StringWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_missing = typeDeserialize.ReadValue<string?, Serde.NullableRefWrap.DeserializeImpl<string, global::Serde.StringWrap>>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case 2:
                    _l_throwmissing = typeDeserialize.ReadValue<string?, Serde.NullableRefWrap.DeserializeImpl<string, global::Serde.StringWrap>>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 2;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b101) != 0b101)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new SetToNull()
        {
            Present = _l_present,
            Missing = _l_missing,
            ThrowMissing = _l_throwmissing,
        };
        return newType;
    }
}