
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize(IDeserializer deserializer)
        {
            bool _l_boolfield = default !;
            char _l_charfield = default !;
            byte _l_bytefield = default !;
            ushort _l_ushortfield = default !;
            uint _l_uintfield = default !;
            ulong _l_ulongfield = default !;
            sbyte _l_sbytefield = default !;
            short _l_shortfield = default !;
            int _l_intfield = default !;
            long _l_longfield = default !;
            string _l_stringfield = default !;
            string? _l_nullstringfield = default !;
            uint[] _l_uintarr = default !;
            int[][] _l_nestedarr = default !;
            System.Collections.Immutable.ImmutableArray<int> _l_intimm = default !;
            Serde.Test.AllInOne.ColorEnum _l_color = default !;
            ushort _r_assignedValid = 0;
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<AllInOne>();
            var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_boolfield = typeDeserialize.ReadValue<bool, global::Serde.BoolWrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 0;
                        break;
                    case 1:
                        _l_charfield = typeDeserialize.ReadValue<char, global::Serde.CharWrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 1;
                        break;
                    case 2:
                        _l_bytefield = typeDeserialize.ReadValue<byte, global::Serde.ByteWrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 2;
                        break;
                    case 3:
                        _l_ushortfield = typeDeserialize.ReadValue<ushort, global::Serde.UInt16Wrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 3;
                        break;
                    case 4:
                        _l_uintfield = typeDeserialize.ReadValue<uint, global::Serde.UInt32Wrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 4;
                        break;
                    case 5:
                        _l_ulongfield = typeDeserialize.ReadValue<ulong, global::Serde.UInt64Wrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 5;
                        break;
                    case 6:
                        _l_sbytefield = typeDeserialize.ReadValue<sbyte, global::Serde.SByteWrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 6;
                        break;
                    case 7:
                        _l_shortfield = typeDeserialize.ReadValue<short, global::Serde.Int16Wrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 7;
                        break;
                    case 8:
                        _l_intfield = typeDeserialize.ReadValue<int, global::Serde.Int32Wrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 8;
                        break;
                    case 9:
                        _l_longfield = typeDeserialize.ReadValue<long, global::Serde.Int64Wrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 9;
                        break;
                    case 10:
                        _l_stringfield = typeDeserialize.ReadValue<string, global::Serde.StringWrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 10;
                        break;
                    case 11:
                        _l_nullstringfield = typeDeserialize.ReadValue<string?, Serde.NullableRefWrap.DeserializeImpl<string, global::Serde.StringWrap>>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 11;
                        break;
                    case 12:
                        _l_uintarr = typeDeserialize.ReadValue<uint[], Serde.ArrayWrap.DeserializeImpl<uint, global::Serde.UInt32Wrap>>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 12;
                        break;
                    case 13:
                        _l_nestedarr = typeDeserialize.ReadValue<int[][], Serde.ArrayWrap.DeserializeImpl<int[], Serde.ArrayWrap.DeserializeImpl<int, global::Serde.Int32Wrap>>>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 13;
                        break;
                    case 14:
                        _l_intimm = typeDeserialize.ReadValue<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayWrap.DeserializeImpl<int, global::Serde.Int32Wrap>>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 14;
                        break;
                    case 15:
                        _l_color = typeDeserialize.ReadValue<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumWrap>(_l_index_);
                        _r_assignedValid |= ((ushort)1) << 15;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }

            if ((_r_assignedValid & 0b1111011111111111) != 0b1111011111111111)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new Serde.Test.AllInOne()
            {
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
                NullStringField = _l_nullstringfield,
                UIntArr = _l_uintarr,
                NestedArr = _l_nestedarr,
                IntImm = _l_intimm,
                Color = _l_color,
            };
            return newType;
        }
    }
}