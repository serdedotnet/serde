//HintName: ComplexRecord.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial record ComplexRecord
{
    sealed partial class _SerdeObj : global::Serde.ISerde<ComplexRecord>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => ComplexRecord.s_serdeInfo;

        void global::Serde.ISerialize<ComplexRecord>.Serialize(ComplexRecord value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.Id);
            _l_type.WriteStringIfNotNull(_l_info, 1, value.Description);
            _l_type.WriteValueIfNotNull<SimpleRecord?, Serde.NullableRefProxy.Ser<SimpleRecord, SimpleRecord>>(_l_info, 2, value.NestedRecord);
            _l_type.End(_l_info);
        }
        async global::System.Threading.Tasks.Task<ComplexRecord> Serde.IDeserialize<ComplexRecord>.Deserialize(IDeserializer deserializer)
        {
            int _l_id = default!;
            string? _l_description = default!;
            SimpleRecord? _l_nestedrecord = default!;

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
                        _l_id = await typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_description = await typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_nestedrecord = await typeDeserialize.ReadValue<SimpleRecord?, Serde.NullableRefProxy.De<SimpleRecord, SimpleRecord>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new ComplexRecord(_l_id, _l_description, _l_nestedrecord) {
            };

            return newType;
        }
    }
}
