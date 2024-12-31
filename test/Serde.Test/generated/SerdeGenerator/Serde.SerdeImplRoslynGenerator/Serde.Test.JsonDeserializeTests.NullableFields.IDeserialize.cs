
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class NullableFields : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.NullableFields>
        {
            static IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields> IDeserializeProvider<Serde.Test.JsonDeserializeTests.NullableFields>.DeserializeInstance => NullableFieldsDeserializeProxy.Instance;

            sealed class NullableFieldsDeserializeProxy : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>
            {
                Serde.Test.JsonDeserializeTests.NullableFields Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>.Deserialize(IDeserializer deserializer)
                {
                    string? _l_s = default !;
                    System.Collections.Generic.Dictionary<string, string?> _l_dict = default !;
                    byte _r_assignedValid = 0;
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<NullableFields>();
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case 0:
                                _l_s = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.Deserialize<string, global::Serde.StringProxy>>(_l_index_);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 1:
                                _l_dict = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.Deserialize<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.Deserialize<string, global::Serde.StringProxy>>>(_l_index_);
                                _r_assignedValid |= ((byte)1) << 1;
                                break;
                            case Serde.IDeserializeType.IndexNotFound:
                                typeDeserialize.SkipValue();
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }

                    if ((_r_assignedValid & 0b10) != 0b10)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.NullableFields()
                    {
                        S = _l_s,
                        Dict = _l_dict,
                    };
                    return newType;
                }

                public static readonly NullableFieldsDeserializeProxy Instance = new();
                private NullableFieldsDeserializeProxy()
                {
                }
            }
        }
    }
}