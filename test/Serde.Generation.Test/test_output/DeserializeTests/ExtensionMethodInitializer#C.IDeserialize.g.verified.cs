//HintName: C.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _DeObj : Serde.IDeserialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            System.Collections.Immutable.ImmutableArray<int> _l_arr = default!;

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
                        _l_arr = typeDeserialize.ReadBoxedValue<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
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
            var newType = new C() {
                Arr = _l_arr,
            };

            return newType;
        }
    }
}
