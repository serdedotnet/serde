//HintName: R.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record R : Serde.IDeserialize<R>
{
    static R Serde.IDeserialize<R>.Deserialize(IDeserializer deserializer)
    {
        int _l_a = default !;
        string _l_b = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<R>();
        var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_a = typeDeserialize.ReadI32(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_b = typeDeserialize.ReadString(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    typeDeserialize.SkipValue();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b11) != 0b11)
        {
            throw Serde.DeserializeException.UnassignedMember();
        }

        var newType = new R(_l_a, _l_b)
        {
        };
        return newType;
    }
}