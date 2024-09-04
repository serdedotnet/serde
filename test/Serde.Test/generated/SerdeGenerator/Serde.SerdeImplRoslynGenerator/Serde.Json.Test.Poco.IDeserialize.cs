﻿
#nullable enable
using System;
using Serde;

namespace Serde.Json.Test
{
    partial class Poco : Serde.IDeserialize<Serde.Json.Test.Poco>
    {
        static Serde.Json.Test.Poco Serde.IDeserialize<Serde.Json.Test.Poco>.Deserialize(IDeserializer deserializer)
        {
            int _l_id = default !;
            byte _r_assignedValid = 0;
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Poco>();
            var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_id = typeDeserialize.ReadValue<int, global::Serde.Int32Wrap>(_l_index_);
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

            var newType = new Serde.Json.Test.Poco()
            {
                Id = _l_id,
            };
            return newType;
        }
    }
}