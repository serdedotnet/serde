//HintName: Original.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct Original : Serde.IDeserialize<Original>
{
    static Original Serde.IDeserialize<Original>.Deserialize(IDeserializer deserializer)
    {
        string _l_name = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Original>();
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_name = typeDeserialize.ReadValue<string, global::Serde.StringWrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b1) != 0b1)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new Original()
        {
            Name = _l_name,
        };
        return newType;
    }
}