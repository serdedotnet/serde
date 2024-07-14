
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct IdStructList : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStructList>
        {
            static Serde.Test.JsonDeserializeTests.IdStructList Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStructList>.Deserialize(IDeserializer deserializer)
            {
                int _l_count = default !;
                System.Collections.Generic.List<Serde.Test.JsonDeserializeTests.IdStruct> _l_list = default !;
                byte _r_assignedValid = 0;
                var _l_serdeInfo = IdStructListSerdeInfo.Instance;
                var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_count = typeDeserialize.ReadValue<int, Int32Wrap>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            _l_list = typeDeserialize.ReadValue<System.Collections.Generic.List<Serde.Test.JsonDeserializeTests.IdStruct>, Serde.ListWrap.DeserializeImpl<Serde.Test.JsonDeserializeTests.IdStruct, Serde.Test.JsonDeserializeTests.IdStruct>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b11) != 0b11)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Serde.Test.JsonDeserializeTests.IdStructList()
                {
                    Count = _l_count,
                    List = _l_list,
                };
                return newType;
            }
        }
    }
}