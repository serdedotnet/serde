
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class TupleTests
{
    partial record TupleHolder
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.TupleTests.TupleHolder>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.TupleTests.TupleHolder.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.TupleTests.TupleHolder>.Serialize(Serde.Test.TupleTests.TupleHolder value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteBoxedValue<(int, string), Serde.TupleProxy.Ser<int, string, global::Serde.I32Proxy, global::Serde.StringProxy>>(_l_info, 0, value.Pair);
                _l_type.WriteBoxedValue<(int, (string, bool)), Serde.TupleProxy.Ser<int, (string, bool), global::Serde.I32Proxy, Serde.TupleProxy.Ser<string, bool, global::Serde.StringProxy, global::Serde.BoolProxy>>>(_l_info, 1, value.Nested);
                _l_type.WriteValue<System.Collections.Generic.List<(int, int)>, Serde.ListProxy.Ser<(int, int), Serde.TupleProxy.Ser<int, int, global::Serde.I32Proxy, global::Serde.I32Proxy>>>(_l_info, 2, value.Points);
                _l_type.End(_l_info);
            }
            Serde.Test.TupleTests.TupleHolder Serde.IDeserialize<Serde.Test.TupleTests.TupleHolder>.Deserialize(IDeserializer deserializer)
            {
                (int, string) _l_pair = default!;
                (int, (string, bool)) _l_nested = default!;
                System.Collections.Generic.List<(int, int)> _l_points = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                while (true)
                {
                    var (_l_index_, _) = typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                    if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                    {
                        break;
                    }

                    switch (_l_index_)
                    {
                        case 0:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 0, _l_serdeInfo);
                            _l_pair = typeDeserialize.ReadBoxedValue<(int, string), Serde.TupleProxy.De<int, string, global::Serde.I32Proxy, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 1:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                            _l_nested = typeDeserialize.ReadBoxedValue<(int, (string, bool)), Serde.TupleProxy.De<int, (string, bool), global::Serde.I32Proxy, Serde.TupleProxy.De<string, bool, global::Serde.StringProxy, global::Serde.BoolProxy>>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                        case 2:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                            _l_points = typeDeserialize.ReadValue<System.Collections.Generic.List<(int, int)>, Serde.ListProxy.De<(int, int), Serde.TupleProxy.De<int, int, global::Serde.I32Proxy, global::Serde.I32Proxy>>>(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 2;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                typeDeserialize.End(_l_serdeInfo);
                if ((_r_assignedValid & 0b111) != 0b111)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.TupleTests.TupleHolder() {
                    Pair = _l_pair,
                    Nested = _l_nested,
                    Points = _l_points,
                };

                return newType;
            }
        }
    }
}
