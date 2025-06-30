//HintName: SetToNull.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial record struct SetToNull
{
    sealed partial class _DeObj : Serde.IDeserialize<SetToNull>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => SetToNull.s_serdeInfo;

        async global::System.Threading.Tasks.ValueTask<SetToNull> Serde.IDeserialize<SetToNull>.Deserialize(IDeserializer deserializer)
        {
            string _l_present = default!;
            string? _l_missing = default!;
            string? _l_throwmissing = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_present = await typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_missing = await typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_throwmissing = await typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b101) != 0b101)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new SetToNull() {
                Present = _l_present,
                Missing = _l_missing,
                ThrowMissing = _l_throwmissing,
            };

            return newType;
        }
    }
}
