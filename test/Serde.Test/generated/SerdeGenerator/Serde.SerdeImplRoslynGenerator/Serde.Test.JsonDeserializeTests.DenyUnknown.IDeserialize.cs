
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct DenyUnknown : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DenyUnknown>
    {
        static IDeserialize<Serde.Test.JsonDeserializeTests.DenyUnknown> IDeserializeProvider<Serde.Test.JsonDeserializeTests.DenyUnknown>.DeserializeInstance
            => DenyUnknownDeserializeProxy.Instance;

        sealed partial class DenyUnknownDeserializeProxy :Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DenyUnknown>
        {
            Serde.Test.JsonDeserializeTests.DenyUnknown Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DenyUnknown>.Deserialize(IDeserializer deserializer)
            {
                string _l_present = default!;
                string? _l_missing = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<DenyUnknown>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_present = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_missing = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.Deserialize<string,global::Serde.StringProxy>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            throw Serde.DeserializeException.UnknownMember(_l_errorName!, _l_serdeInfo);
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.JsonDeserializeTests.DenyUnknown() {
                    Present = _l_present,
                    Missing = _l_missing,
                };

                return newType;
            }
            public static readonly DenyUnknownDeserializeProxy Instance = new();
            private DenyUnknownDeserializeProxy() { }

        }
    }
}
