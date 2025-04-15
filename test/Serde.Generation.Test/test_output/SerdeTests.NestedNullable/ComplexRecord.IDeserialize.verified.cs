//HintName: ComplexRecord.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial record ComplexRecord : Serde.IDeserializeProvider<ComplexRecord>
{
    static IDeserialize<ComplexRecord> IDeserializeProvider<ComplexRecord>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<ComplexRecord>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => ComplexRecord.s_serdeInfo;

        ComplexRecord Serde.IDeserialize<ComplexRecord>.Deserialize(IDeserializer deserializer)
        {
            int _l_id = default!;
            string? _l_description = default!;
            SimpleRecord? _l_nestedrecord = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_id = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_description = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_nestedrecord = typeDeserialize.ReadValue<SimpleRecord?, Serde.NullableRefProxy.De<SimpleRecord, SimpleRecord>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
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
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
