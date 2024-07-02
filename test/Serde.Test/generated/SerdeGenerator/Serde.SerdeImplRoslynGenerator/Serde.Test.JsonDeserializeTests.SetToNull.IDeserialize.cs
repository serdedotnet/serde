
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct SetToNull : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>
        {
            static Serde.Test.JsonDeserializeTests.SetToNull Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>.Deserialize(IDeserializer deserializer)
            {
                string _l_present = default !;
                string? _l_missing = default !;
                byte _r_assignedValid = 0;
                var _l_typeInfo = SetToNullSerdeTypeInfo.TypeInfo;
                var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_present = typeDeserialize.ReadValue<string, StringWrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_missing = typeDeserialize.ReadValue<string?, Serde.NullableRefWrap.DeserializeImpl<string, StringWrap>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
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

                var newType = new Serde.Test.JsonDeserializeTests.SetToNull()
                {
                    Present = _l_present,
                    Missing = _l_missing,
                };
                return newType;
            }
        }
    }
}