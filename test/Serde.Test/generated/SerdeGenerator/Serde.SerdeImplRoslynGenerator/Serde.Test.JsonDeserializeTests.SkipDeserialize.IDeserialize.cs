﻿
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record SkipDeserialize : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SkipDeserialize>
        {
            static Serde.Test.JsonDeserializeTests.SkipDeserialize Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SkipDeserialize>.Deserialize(IDeserializer deserializer)
            {
                string _l_required = default !;
                byte _r_assignedValid = 0;
                var _l_typeInfo = SkipDeserializeSerdeTypeInfo.TypeInfo;
                var typeDeserialize = deserializer.DeserializeType(_l_typeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_typeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_required = typeDeserialize.ReadValue<string, StringWrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
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

                var newType = new Serde.Test.JsonDeserializeTests.SkipDeserialize()
                {
                    Required = _l_required,
                };
                return newType;
            }
        }
    }
}