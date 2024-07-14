//HintName: S.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize(IDeserializer deserializer)
    {
        System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS> _l_opts = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = SSerdeInfo.Instance;
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_opts = typeDeserialize.ReadValue<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayWrap.DeserializeImpl<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OptsWrap>>(_l_index_);
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

        var newType = new S()
        {
            Opts = _l_opts,
        };
        return newType;
    }
}