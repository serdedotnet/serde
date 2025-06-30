//HintName: ArgumentInfo.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class ArgumentInfo
{
    sealed partial class _SerdeObj : global::Serde.ISerde<ArgumentInfo>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => ArgumentInfo.s_serdeInfo;

        void global::Serde.ISerialize<ArgumentInfo>.Serialize(ArgumentInfo value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteString(_l_info, 0, value.Name);
            _l_type.WriteString(_l_info, 1, value.Value);
            _l_type.End(_l_info);
        }
        async global::System.Threading.Tasks.ValueTask<ArgumentInfo> Serde.IDeserialize<ArgumentInfo>.Deserialize(IDeserializer deserializer)
        {
            string _l_name = default!;
            string _l_value = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_name = await typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_value = await typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
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
            var newType = new ArgumentInfo() {
                Name = _l_name,
                Value = _l_value,
            };

            return newType;
        }
    }
}
