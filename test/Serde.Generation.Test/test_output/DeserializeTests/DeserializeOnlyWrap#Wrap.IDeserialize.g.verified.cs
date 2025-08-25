//HintName: Wrap.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial record struct Wrap
{
    sealed partial class _DeObj : Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Wrap.s_serdeInfo;

        System.Runtime.InteropServices.ComTypes.BIND_OPTS Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Deserialize(IDeserializer deserializer)
        {
            int _l_cbstruct = default!;
            int _l_dwtickcountdeadline = default!;
            int _l_grfflags = default!;
            int _l_grfmode = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            while (true)
            {
                var (_l_index_, _) = typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                {
                    break;
                }

                switch (_l_index_)
                {
                    case 0:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 0, _l_serdeInfo);
                        _l_cbstruct = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_dwtickcountdeadline = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                        _l_grfflags = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
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
