//HintName: ArrayField.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class ArrayField : Serde.IDeserialize<ArrayField>
{
    static ArrayField Serde.IDeserialize<ArrayField>.Deserialize(IDeserializer deserializer)
    {
        int[] _l_intarr = default !;
        byte _r_assignedValid = 0b0;
        var _l_typeInfo = ArrayFieldSerdeTypeInfo.TypeInfo;
        var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_intarr = typeDeserialize.ReadValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if (_r_assignedValid != 0b1)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new ArrayField()
        {
            IntArr = _l_intarr,
        };
        return newType;
    }
}