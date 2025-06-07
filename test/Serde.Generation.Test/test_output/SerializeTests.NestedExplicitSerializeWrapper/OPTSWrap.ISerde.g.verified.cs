//HintName: OPTSWrap.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class OPTSWrap
{
    sealed partial class _SerdeObj : global::Serde.ISerde<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => OPTSWrap.s_serdeInfo;

        void global::Serde.ISerialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Serialize(System.Runtime.InteropServices.ComTypes.BIND_OPTS value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.cbStruct);
            _l_type.WriteI32(_l_info, 1, value.dwTickCountDeadline);
            _l_type.WriteI32(_l_info, 2, value.grfFlags);
            _l_type.WriteI32(_l_info, 3, value.grfMode);
            _l_type.End(_l_info);
        }
        System.Runtime.InteropServices.ComTypes.BIND_OPTS Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Deserialize(IDeserializer deserializer)
        {
            int _l_cbstruct = default!;
            int _l_dwtickcountdeadline = default!;
            int _l_grfflags = default!;
            int _l_grfmode = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_cbstruct = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_dwtickcountdeadline = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_grfflags = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        _l_grfmode = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1111) != 0b1111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new System.Runtime.InteropServices.ComTypes.BIND_OPTS() {
                cbStruct = _l_cbstruct,
                dwTickCountDeadline = _l_dwtickcountdeadline,
                grfFlags = _l_grfflags,
                grfMode = _l_grfmode,
            };

            return newType;
        }
    }
}
