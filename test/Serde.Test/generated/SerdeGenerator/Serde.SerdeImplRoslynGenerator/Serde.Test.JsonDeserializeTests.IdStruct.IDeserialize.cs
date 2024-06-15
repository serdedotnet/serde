
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial struct IdStruct : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStruct>
        {
            static Serde.Test.JsonDeserializeTests.IdStruct Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStruct>.Deserialize(IDeserializer deserializer)
            {
                int _l_id = default !;
                byte _r_assignedValid = 0;
                var _l_typeInfo = IdStructSerdeTypeInfo.TypeInfo;
                var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_id = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Serde.Test.JsonDeserializeTests.IdStruct()
                {
                    Id = _l_id,
                };
                return newType;
            }
        }
    }
}