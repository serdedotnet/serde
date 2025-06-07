//HintName: InvalidProxyTest.ISerde.g.cs

#nullable enable

using System;
using Serde;
partial class InvalidProxyTest<T>
{
    sealed partial class _SerdeObj : global::Serde.ISerde<InvalidProxyTest<T>>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => InvalidProxyTest<T>.s_serdeInfo;

        void global::Serde.ISerialize<InvalidProxyTest<T>>.Serialize(InvalidProxyTest<T> value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.End(_l_info);
        }
        InvalidProxyTest<T> Serde.IDeserialize<InvalidProxyTest<T>>.Deserialize(IDeserializer deserializer)
        {
            T? _l_value1 = default!;
            T? _l_value2 = default!;
            T? _l_value3 = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_value1 = typeDeserialize.ReadValue<T?, T?>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        _l_value2 = typeDeserialize.ReadValue<T?, T?>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        _l_value3 = typeDeserialize.ReadValue<T?, T?>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b0) != 0b0)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new InvalidProxyTest<T>() {
                Value1 = _l_value1,
                Value2 = _l_value2,
                Value3 = _l_value3,
            };

            return newType;
        }
    }
}
