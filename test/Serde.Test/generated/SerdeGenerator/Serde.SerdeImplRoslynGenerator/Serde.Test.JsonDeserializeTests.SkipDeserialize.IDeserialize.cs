
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record SkipDeserialize : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SkipDeserialize>
    {
        static IDeserialize<Serde.Test.JsonDeserializeTests.SkipDeserialize> IDeserializeProvider<Serde.Test.JsonDeserializeTests.SkipDeserialize>.DeserializeInstance
            => SkipDeserializeDeserializeProxy.Instance;

        sealed partial class SkipDeserializeDeserializeProxy :Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SkipDeserialize>
        {
            Serde.Test.JsonDeserializeTests.SkipDeserialize Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SkipDeserialize>.Deserialize(IDeserializer deserializer)
            {
                string _l_required = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<SkipDeserialize>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_required = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
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
                var newType = new Serde.Test.JsonDeserializeTests.SkipDeserialize() {
                    Required = _l_required,
                };

                return newType;
            }
            public static readonly SkipDeserializeDeserializeProxy Instance = new();
            private SkipDeserializeDeserializeProxy() { }

        }
    }
}
