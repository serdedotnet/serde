//HintName: OPTSWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct OPTSWrap : Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    static System.Runtime.InteropServices.ComTypes.BIND_OPTS Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Deserialize(IDeserializer deserializer)
    {
        int _l_cbstruct = default !;
        int _l_dwtickcountdeadline = default !;
        int _l_grfflags = default !;
        int _l_grfmode = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = BIND_OPTSSerdeInfo.Instance;
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_cbstruct = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case 1:
                    _l_dwtickcountdeadline = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 1;
                    break;
                case 2:
                    _l_grfflags = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 2;
                    break;
                case 3:
                    _l_grfmode = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 3;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b1111) != 0b1111)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new System.Runtime.InteropServices.ComTypes.BIND_OPTS()
        {
            cbStruct = _l_cbstruct,
            dwTickCountDeadline = _l_dwtickcountdeadline,
            grfFlags = _l_grfflags,
            grfMode = _l_grfmode,
        };
        return newType;
    }
}