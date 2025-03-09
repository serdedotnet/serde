﻿//HintName: ArrayField.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial class ArrayField : Serde.IDeserializeProvider<ArrayField>
{
    static IDeserialize<ArrayField> IDeserializeProvider<ArrayField>.DeserializeInstance
        => ArrayFieldDeserializeProxy.Instance;

    sealed partial class ArrayFieldDeserializeProxy :Serde.IDeserialize<ArrayField>
    {
        ArrayField Serde.IDeserialize<ArrayField>.Deserialize(IDeserializer deserializer)
        {
            int[] _l_intarr = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ArrayField>();
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_intarr = typeDeserialize.ReadValue<int[], Serde.ArrayProxy.Deserialize<int, global::Serde.I32Proxy>>(_l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        typeDeserialize.SkipValue();
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new ArrayField() {
                IntArr = _l_intarr,
            };

            return newType;
        }
        public static readonly ArrayFieldDeserializeProxy Instance = new();
        private ArrayFieldDeserializeProxy() { }

    }
}
