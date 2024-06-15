//HintName: Test.RecursiveWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct RecursiveWrap : Serde.IDeserialize<Recursive>
    {
        static Recursive Serde.IDeserialize<Recursive>.Deserialize(IDeserializer deserializer)
        {
            Recursive? _l_next = default !;
            byte _r_assignedValid = 0b1;
            var _l_typeInfo = RecursiveSerdeTypeInfo.TypeInfo;
            var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_next = typeDeserialize.ReadValue<Recursive?, RecursiveWrap>(_l_index_);
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

            var newType = new Recursive()
            {
                Next = _l_next,
            };
            return newType;
        }
    }
}