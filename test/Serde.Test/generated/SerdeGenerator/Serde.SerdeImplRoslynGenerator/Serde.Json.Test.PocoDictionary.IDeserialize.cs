
#nullable enable
using System;
using Serde;

namespace Serde.Json.Test
{
    partial class PocoDictionary : Serde.IDeserialize<Serde.Json.Test.PocoDictionary>
    {
        static Serde.Json.Test.PocoDictionary Serde.IDeserialize<Serde.Json.Test.PocoDictionary>.Deserialize(IDeserializer deserializer)
        {
            System.Collections.Generic.Dictionary<string, string> _l_key = default !;
            byte _r_assignedValid = 0;
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<PocoDictionary>();
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_key = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, string>, Serde.DictWrap.DeserializeImpl<string, global::Serde.StringWrap, string, global::Serde.StringWrap>>(_l_index_);
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

            var newType = new Serde.Json.Test.PocoDictionary()
            {
                key = _l_key,
            };
            return newType;
        }
    }
}