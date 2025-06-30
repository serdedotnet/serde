//HintName: Test.RecursiveWrap.ISerde.g.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial class RecursiveWrap
{
    sealed partial class _SerdeObj : global::Serde.ISerde<Recursive>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Test.RecursiveWrap.s_serdeInfo;

        void global::Serde.ISerialize<Recursive>.Serialize(Recursive value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValueIfNotNull<Recursive?, Test.RecursiveWrap>(_l_info, 0, value.Next);
            _l_type.End(_l_info);
        }
        async global::System.Threading.Tasks.Task<Recursive> Serde.IDeserialize<Recursive>.Deserialize(IDeserializer deserializer)
        {
            Recursive? _l_next = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            while (true)
            {
                var (_l_index_, _) = await typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                {
                    break;
                }

                switch (_l_index_)
                {
                    case 0:
                        _l_next = await typeDeserialize.ReadValue<Recursive?, Test.RecursiveWrap>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b0) != 0b0)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Recursive() {
                Next = _l_next,
            };

            return newType;
        }
    }
}
