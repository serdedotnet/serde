//HintName: SimpleRecord.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial record SimpleRecord
{
    sealed partial class _SerdeObj : global::Serde.ISerde<SimpleRecord>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => SimpleRecord.s_serdeInfo;

        void global::Serde.ISerialize<SimpleRecord>.Serialize(SimpleRecord value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.Id);
            _l_type.WriteString(_l_info, 1, value.Name);
            _l_type.End(_l_info);
        }
        async global::System.Threading.Tasks.Task<SimpleRecord> Serde.IDeserialize<SimpleRecord>.Deserialize(IDeserializer deserializer)
        {
            int _l_id = default!;
            string _l_name = default!;

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
                        _l_name = await typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b11) != 0b11)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new SimpleRecord(_l_id, _l_name) {
            };

            return newType;
        }
    }
}
