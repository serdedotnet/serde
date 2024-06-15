
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class NullableFields : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>
        {
            static Serde.Test.JsonDeserializeTests.NullableFields Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>.Deserialize(IDeserializer deserializer)
            {
                string? _l_s = default !;
                System.Collections.Generic.Dictionary<string, string?> _l_dict = default !;
                byte _r_assignedValid = 0;
                var _l_typeInfo = NullableFieldsSerdeTypeInfo.TypeInfo;
                var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_s = typeDeserialize.ReadValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_dict = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, string?>, DictWrap.DeserializeImpl<string, StringWrap, string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b10) != 0b10)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Serde.Test.JsonDeserializeTests.NullableFields()
                {
                    S = _l_s,
                    Dict = _l_dict,
                };
                return newType;
            }
        }
    }
}