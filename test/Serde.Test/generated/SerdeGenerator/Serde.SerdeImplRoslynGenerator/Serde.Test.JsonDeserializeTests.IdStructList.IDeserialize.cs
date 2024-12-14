
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct IdStructList : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.IdStructList>
        {
            static IDeserialize<Serde.Test.JsonDeserializeTests.IdStructList> IDeserializeProvider<Serde.Test.JsonDeserializeTests.IdStructList>.DeserializeInstance => IdStructListDeserializeProxy.Instance;

            sealed class IdStructListDeserializeProxy : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStructList>
            {
                Serde.Test.JsonDeserializeTests.IdStructList Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStructList>.Deserialize(IDeserializer deserializer)
                {
                    int _l_count = default !;
                    System.Collections.Generic.List<Serde.Test.JsonDeserializeTests.IdStruct> _l_list = default !;
                    byte _r_assignedValid = 0;
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<IdStructList>();
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case 0:
                                _l_count = typeDeserialize.ReadI32(_l_index_);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 1:
                                _l_list = typeDeserialize.ReadValue<System.Collections.Generic.List<Serde.Test.JsonDeserializeTests.IdStruct>, Serde.ListProxy.Deserialize<Serde.Test.JsonDeserializeTests.IdStruct, Serde.Test.JsonDeserializeTests.IdStruct>>(_l_index_);
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

                    var newType = new Serde.Test.JsonDeserializeTests.IdStructList()
                    {
                        Count = _l_count,
                        List = _l_list,
                    };
                    return newType;
                }

                public static readonly IdStructListDeserializeProxy Instance = new();
                private IdStructListDeserializeProxy()
                {
                }
            }
        }
    }
}