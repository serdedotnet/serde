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
        var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_name = typeDeserialize.ReadString(_l_index_);
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

        var newType = new Original()
        {
            Name = _l_name,
        };
        return newType;
    }
}