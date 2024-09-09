
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct ThrowMissing : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing>
        {
            static Serde.Test.JsonDeserializeTests.ThrowMissing Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing>.Deserialize(IDeserializer deserializer)
            {
                string _l_present = default !;
                string? _l_missing = default !;
                byte _r_assignedValid = 0;
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ThrowMissing>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_present = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_missing = typeDeserialize.ReadValue<string?, Serde.NullableRefWrap.DeserializeImpl<string, global::Serde.StringWrap>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            typeDeserialize.SkipValue();
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b11) != 0b11)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }

                var newType = new Serde.Test.JsonDeserializeTests.ThrowMissing()
                {
                    Present = _l_present,
                    Missing = _l_missing,
                };
                return newType;
            }
        }
    }
}