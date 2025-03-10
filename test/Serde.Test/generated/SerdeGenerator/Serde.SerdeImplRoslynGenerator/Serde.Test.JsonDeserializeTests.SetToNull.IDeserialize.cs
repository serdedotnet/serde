
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct SetToNull : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SetToNull>
    {
        static IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull> IDeserializeProvider<Serde.Test.JsonDeserializeTests.SetToNull>.DeserializeInstance
            => SetToNullDeserializeProxy.Instance;

        sealed partial class SetToNullDeserializeProxy :Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>
        {
            Serde.Test.JsonDeserializeTests.SetToNull Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>.Deserialize(IDeserializer deserializer)
            {
                string _l_present = default!;
                string? _l_missing = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<SetToNull>();
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
                            _l_missing = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.Deserialize<string, global::Serde.StringProxy>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
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
                var newType = new Serde.Test.JsonDeserializeTests.SetToNull() {
                    Present = _l_present,
                    Missing = _l_missing,
                };

                return newType;
            }
            public static readonly SetToNullDeserializeProxy Instance = new();
            private SetToNullDeserializeProxy() { }

        }
    }
}
