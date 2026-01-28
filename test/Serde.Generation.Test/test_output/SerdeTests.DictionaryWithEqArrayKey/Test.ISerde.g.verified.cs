//HintName: Test.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class Test
{
    sealed partial class _SerdeObj : global::Serde.ISerde<Test>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Test.s_serdeInfo;

        void global::Serde.ISerialize<Test>.Serialize(Test value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValue<System.Collections.Generic.Dictionary<EqArray<int>, int>, Serde.DictProxy.Ser<EqArray<int>, int, EqArrayProxy.Ser<int, global::Serde.I32Proxy>, global::Serde.I32Proxy>>(_l_info, 0, value.data);
            _l_type.End(_l_info);
        }
        Test Serde.IDeserialize<Test>.Deserialize(IDeserializer deserializer)
        {
            System.Collections.Generic.Dictionary<EqArray<int>, int> _l_data = default!;

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
                        _l_data = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<EqArray<int>, int>, Serde.DictProxy.De<EqArray<int>, int, EqArrayProxy.De<int, global::Serde.I32Proxy>, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Test() {
                data = _l_data,
            };

            return newType;
        }
    }
}
