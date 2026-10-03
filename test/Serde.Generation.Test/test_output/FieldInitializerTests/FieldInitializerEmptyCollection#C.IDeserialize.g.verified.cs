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
            System.Collections.Generic.List<int> _l_items = [];
            int[] _l_arr = [];
            string[] _l_strs = [];
            int[] _l_nonempty = default!;

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
                        _l_items = typeDeserialize.ReadValue<System.Collections.Generic.List<int>, Serde.ListProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_arr = typeDeserialize.ReadValue<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 2:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                        _l_strs = typeDeserialize.ReadValue<string[], Serde.ArrayProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 3:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
                        _l_nonempty = typeDeserialize.ReadValue<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            typeDeserialize.End(_l_serdeInfo);
            if ((_r_assignedValid & 0b1000) != 0b1000)
            {
                throw Serde.DeserializeException.UnassignedMember(_r_assignedValid, 0b1000, _l_serdeInfo);
            }
            var newType = new C() {
                Items = _l_items,
                Arr = _l_arr,
                Strs = _l_strs,
                NonEmpty = _l_nonempty,
            };

            return newType;
        }
    }
}
