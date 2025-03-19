
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne : Serde.IDeserializeProvider<Serde.Test.AllInOne>
{
    static IDeserialize<Serde.Test.AllInOne> IDeserializeProvider<Serde.Test.AllInOne>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<Serde.Test.AllInOne>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.AllInOne.s_serdeInfo;

        Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize(IDeserializer deserializer)
        {
            bool _l_boolfield = default!;
            char _l_charfield = default!;
            byte _l_bytefield = default!;
            ushort _l_ushortfield = default!;
            uint _l_uintfield = default!;
            ulong _l_ulongfield = default!;
            sbyte _l_sbytefield = default!;
            short _l_shortfield = default!;
            int _l_intfield = default!;
            long _l_longfield = default!;
            string _l_stringfield = default!;
            string _l_escapedstringfield = default!;
            string? _l_nullstringfield = default!;
            uint[] _l_uintarr = default!;
            int[][] _l_nestedarr = default!;
            System.Collections.Immutable.ImmutableArray<int> _l_intimm = default!;
            Serde.Test.AllInOne.ColorEnum _l_color = default!;

            uint _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_boolfield = typeDeserialize.ReadBool(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 0;
                        break;
                    case 1:
                        _l_charfield = typeDeserialize.ReadChar(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 1;
                        break;
                    case 2:
                        _l_bytefield = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 2;
                        break;
                    case 3:
                        _l_ushortfield = typeDeserialize.ReadU16(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 3;
                        break;
                    case 4:
                        _l_uintfield = typeDeserialize.ReadU32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 4;
                        break;
                    case 5:
                        _l_ulongfield = typeDeserialize.ReadU64(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 5;
                        break;
                    case 6:
                        _l_sbytefield = typeDeserialize.ReadI8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 6;
                        break;
                    case 7:
                        _l_shortfield = typeDeserialize.ReadI16(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 7;
                        break;
                    case 8:
                        _l_intfield = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 8;
                        break;
                    case 9:
                        _l_longfield = typeDeserialize.ReadI64(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 9;
                        break;
                    case 10:
                        _l_stringfield = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 10;
                        break;
                    case 11:
                        _l_escapedstringfield = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 11;
                        break;
                    case 12:
                        _l_nullstringfield = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 12;
                        break;
                    case 13:
                        _l_uintarr = typeDeserialize.ReadValue<uint[], Serde.ArrayProxy.De<uint, global::Serde.U32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 13;
                        break;
                    case 14:
                        _l_nestedarr = typeDeserialize.ReadValue<int[][], Serde.ArrayProxy.De<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 14;
                        break;
                    case 15:
                        _l_intimm = typeDeserialize.ReadBoxedValue<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 15;
                        break;
                    case 16:
                        _l_color = typeDeserialize.ReadBoxedValue<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 16;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b11110111111111111) != 0b11110111111111111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Serde.Test.AllInOne() {
                BoolField = _l_boolfield,
                CharField = _l_charfield,
                ByteField = _l_bytefield,
                UShortField = _l_ushortfield,
                UIntField = _l_uintfield,
                ULongField = _l_ulongfield,
                SByteField = _l_sbytefield,
                ShortField = _l_shortfield,
                IntField = _l_intfield,
                LongField = _l_longfield,
                StringField = _l_stringfield,
                EscapedStringField = _l_escapedstringfield,
                NullStringField = _l_nullstringfield,
                UIntArr = _l_uintarr,
                NestedArr = _l_nestedarr,
                IntImm = _l_intimm,
                Color = _l_color,
            };

            return newType;
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
