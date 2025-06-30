
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class NullableFields
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonDeserializeTests.NullableFields.s_serdeInfo;

            async global::System.Threading.Tasks.ValueTask<Serde.Test.JsonDeserializeTests.NullableFields> Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>.Deserialize(IDeserializer deserializer)
            {
                string? _l_s = default!;
                System.Collections.Generic.Dictionary<string, string?> _l_dict = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_s = await typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_dict = await typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.De<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b10) != 0b10)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.JsonDeserializeTests.NullableFields() {
                    S = _l_s,
                    Dict = _l_dict,
                };

                return newType;
            }
        }
    }
}
