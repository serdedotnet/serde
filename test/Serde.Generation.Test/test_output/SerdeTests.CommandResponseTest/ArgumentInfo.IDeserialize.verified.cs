//HintName: ArgumentInfo.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial class ArgumentInfo : Serde.IDeserializeProvider<ArgumentInfo>
{
    static IDeserialize<ArgumentInfo> IDeserializeProvider<ArgumentInfo>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<ArgumentInfo>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => ArgumentInfo.s_serdeInfo;

        ArgumentInfo Serde.IDeserialize<ArgumentInfo>.Deserialize(IDeserializer deserializer)
        {
            string _l_name = default!;
            string _l_value = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_name = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_value = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
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
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
