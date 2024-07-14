//HintName: C.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
    {
        System.Runtime.InteropServices.ComTypes.BIND_OPTS _l_s = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = CSerdeInfo.Instance;
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_s = typeDeserialize.ReadValue<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>(_l_index_);
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

        var newType = new C()
        {
            S = _l_s,
        };
        return newType;
    }
}