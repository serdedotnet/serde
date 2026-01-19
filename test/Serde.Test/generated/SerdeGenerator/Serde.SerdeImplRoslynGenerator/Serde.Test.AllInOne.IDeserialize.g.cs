
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne
{
    sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.AllInOne>
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
            System.UInt128 _l_uint128field = default!;
            sbyte _l_sbytefield = default!;
            short _l_shortfield = default!;
            int _l_intfield = default!;
            long _l_longfield = default!;
            System.Int128 _l_int128field = default!;
            string _l_stringfield = default!;
            System.DateTimeOffset _l_datetimeoffsetfield = default!;
            System.DateTime _l_datetimefield = default!;
            System.DateOnly _l_dateonlyfield = default!;
            System.TimeOnly _l_timeonlyfield = default!;
            System.Guid _l_guidfield = default!;
            string _l_escapedstringfield = default!;
            string? _l_nullstringfield = default!;
            uint[] _l_uintarr = default!;
            int[][] _l_nestedarr = default!;
            byte[] _l_bytearr = default!;
            System.Collections.Immutable.ImmutableArray<int> _l_intimm = default!;
            Serde.Test.AllInOne.ColorEnum _l_color = default!;

            uint _r_assignedValid = 0;

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
                        _l_boolfield = typeDeserialize.ReadBool(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 0;
                        break;
                    case 1:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 1, _l_serdeInfo);
                        _l_charfield = typeDeserialize.ReadChar(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 1;
                        break;
                    case 2:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 2, _l_serdeInfo);
                        _l_bytefield = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 2;
                        break;
                    case 3:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 3, _l_serdeInfo);
                        _l_ushortfield = typeDeserialize.ReadU16(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 3;
                        break;
                    case 4:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 4, _l_serdeInfo);
                        _l_uintfield = typeDeserialize.ReadU32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 4;
                        break;
                    case 5:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 5, _l_serdeInfo);
                        _l_ulongfield = typeDeserialize.ReadU64(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 5;
                        break;
                    case 6:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 6, _l_serdeInfo);
                        _l_uint128field = typeDeserialize.ReadU128(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 6;
                        break;
                    case 7:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 7, _l_serdeInfo);
                        _l_sbytefield = typeDeserialize.ReadI8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 7;
                        break;
                    case 8:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 8, _l_serdeInfo);
                        _l_shortfield = typeDeserialize.ReadI16(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 8;
                        break;
                    case 9:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 9, _l_serdeInfo);
                        _l_intfield = typeDeserialize.ReadI32(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 9;
                        break;
                    case 10:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 10, _l_serdeInfo);
                        _l_longfield = typeDeserialize.ReadI64(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 10;
                        break;
                    case 11:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 11, _l_serdeInfo);
                        _l_int128field = typeDeserialize.ReadI128(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 11;
                        break;
                    case 12:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 12, _l_serdeInfo);
                        _l_stringfield = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 12;
                        break;
                    case 13:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 13, _l_serdeInfo);
                        _l_datetimeoffsetfield = typeDeserialize.ReadBoxedValue<System.DateTimeOffset, global::Serde.DateTimeOffsetProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 13;
                        break;
                    case 14:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 14, _l_serdeInfo);
                        _l_datetimefield = typeDeserialize.ReadDateTime(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 14;
                        break;
                    case 15:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 15, _l_serdeInfo);
                        _l_dateonlyfield = typeDeserialize.ReadBoxedValue<System.DateOnly, global::Serde.DateOnlyProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 15;
                        break;
                    case 16:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 16, _l_serdeInfo);
                        _l_timeonlyfield = typeDeserialize.ReadBoxedValue<System.TimeOnly, global::Serde.TimeOnlyProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 16;
                        break;
                    case 17:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 17, _l_serdeInfo);
                        _l_guidfield = typeDeserialize.ReadGuid<global::Serde.GuidProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 17;
                        break;
                    case 18:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 18, _l_serdeInfo);
                        _l_escapedstringfield = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 18;
                        break;
                    case 19:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 19, _l_serdeInfo);
                        _l_nullstringfield = typeDeserialize.ReadValue<string?, Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 19;
                        break;
                    case 20:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 20, _l_serdeInfo);
                        _l_uintarr = typeDeserialize.ReadValue<uint[], Serde.ArrayProxy.De<uint, global::Serde.U32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 20;
                        break;
                    case 21:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 21, _l_serdeInfo);
                        _l_nestedarr = typeDeserialize.ReadValue<int[][], Serde.ArrayProxy.De<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 21;
                        break;
                    case 22:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 22, _l_serdeInfo);
                        _l_bytearr = typeDeserialize.ReadValue<byte[], global::Serde.ByteArrayProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 22;
                        break;
                    case 23:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 23, _l_serdeInfo);
                        _l_intimm = typeDeserialize.ReadBoxedValue<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.De<int, global::Serde.I32Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 23;
                        break;
                    case 24:
                        Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 24, _l_serdeInfo);
                        _l_color = typeDeserialize.ReadBoxedValue<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((uint)1) << 24;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1111101111111111111111111) != 0b1111101111111111111111111)
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
                UInt128Field = _l_uint128field,
                SByteField = _l_sbytefield,
                ShortField = _l_shortfield,
                IntField = _l_intfield,
                LongField = _l_longfield,
                Int128Field = _l_int128field,
                StringField = _l_stringfield,
                DateTimeOffsetField = _l_datetimeoffsetfield,
                DateTimeField = _l_datetimefield,
                DateOnlyField = _l_dateonlyfield,
                TimeOnlyField = _l_timeonlyfield,
                GuidField = _l_guidfield,
                EscapedStringField = _l_escapedstringfield,
                NullStringField = _l_nullstringfield,
                UIntArr = _l_uintarr,
                NestedArr = _l_nestedarr,
                ByteArr = _l_bytearr,
                IntImm = _l_intimm,
                Color = _l_color,
            };

            return newType;
        }
    }
}
