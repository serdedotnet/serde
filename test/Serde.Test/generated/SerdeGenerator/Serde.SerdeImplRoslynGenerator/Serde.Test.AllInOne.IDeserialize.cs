
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "BoolField",
                "CharField",
                "ByteField",
                "UShortField",
                "UIntField",
                "ULongField",
                "SByteField",
                "ShortField",
                "IntField",
                "LongField",
                "StringField",
                "NullStringField",
                "UIntArr",
                "NestedArr",
                "IntImm",
                "Color"
            };
            return deserializer.DeserializeType<Serde.Test.AllInOne, SerdeVisitor>("AllInOne", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne";

            private struct FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
            {
                public static byte Deserialize<D>(ref D deserializer)
                    where D : IDeserializer => deserializer.DeserializeString<byte, FieldNameVisitor>(new FieldNameVisitor());
                public string ExpectedTypeName => "string";

                byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
                public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
                {
                    switch (s[0])
                    {
                        case (byte)'b'when s.SequenceEqual("boolField"u8):
                            return 1;
                        case (byte)'c'when s.SequenceEqual("charField"u8):
                            return 2;
                        case (byte)'b'when s.SequenceEqual("byteField"u8):
                            return 3;
                        case (byte)'u'when s.SequenceEqual("uShortField"u8):
                            return 4;
                        case (byte)'u'when s.SequenceEqual("uIntField"u8):
                            return 5;
                        case (byte)'u'when s.SequenceEqual("uLongField"u8):
                            return 6;
                        case (byte)'s'when s.SequenceEqual("sByteField"u8):
                            return 7;
                        case (byte)'s'when s.SequenceEqual("shortField"u8):
                            return 8;
                        case (byte)'i'when s.SequenceEqual("intField"u8):
                            return 9;
                        case (byte)'l'when s.SequenceEqual("longField"u8):
                            return 10;
                        case (byte)'s'when s.SequenceEqual("stringField"u8):
                            return 11;
                        case (byte)'n'when s.SequenceEqual("nullStringField"u8):
                            return 12;
                        case (byte)'u'when s.SequenceEqual("uIntArr"u8):
                            return 13;
                        case (byte)'n'when s.SequenceEqual("nestedArr"u8):
                            return 14;
                        case (byte)'i'when s.SequenceEqual("intImm"u8):
                            return 15;
                        case (byte)'c'when s.SequenceEqual("color"u8):
                            return 16;
                        default:
                            return 0;
                    }
                }
            }

            Serde.Test.AllInOne Serde.IDeserializeVisitor<Serde.Test.AllInOne>.VisitDictionary<D>(ref D d)
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
                ushort _r_assignedValid = 0b100000000000;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_boolfield = d.GetNextValue<bool, BoolWrap>();
                            _r_assignedValid |= ((ushort)1) << 0;
                            break;
                        case 2:
                            _l_charfield = d.GetNextValue<char, CharWrap>();
                            _r_assignedValid |= ((ushort)1) << 1;
                            break;
                        case 3:
                            _l_bytefield = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ushort)1) << 2;
                            break;
                        case 4:
                            _l_ushortfield = d.GetNextValue<ushort, UInt16Wrap>();
                            _r_assignedValid |= ((ushort)1) << 3;
                            break;
                        case 5:
                            _l_uintfield = d.GetNextValue<uint, UInt32Wrap>();
                            _r_assignedValid |= ((ushort)1) << 4;
                            break;
                        case 6:
                            _l_ulongfield = d.GetNextValue<ulong, UInt64Wrap>();
                            _r_assignedValid |= ((ushort)1) << 5;
                            break;
                        case 7:
                            _l_sbytefield = d.GetNextValue<sbyte, SByteWrap>();
                            _r_assignedValid |= ((ushort)1) << 6;
                            break;
                        case 8:
                            _l_shortfield = d.GetNextValue<short, Int16Wrap>();
                            _r_assignedValid |= ((ushort)1) << 7;
                            break;
                        case 9:
                            _l_intfield = d.GetNextValue<int, Int32Wrap>();
                            _r_assignedValid |= ((ushort)1) << 8;
                            break;
                        case 10:
                            _l_longfield = d.GetNextValue<long, Int64Wrap>();
                            _r_assignedValid |= ((ushort)1) << 9;
                            break;
                        case 11:
                            _l_stringfield = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 10;
                            break;
                        case 12:
                            _l_nullstringfield = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                            _r_assignedValid |= ((ushort)1) << 11;
                            break;
                        case 13:
                            _l_uintarr = d.GetNextValue<uint[], ArrayWrap.DeserializeImpl<uint, UInt32Wrap>>();
                            _r_assignedValid |= ((ushort)1) << 12;
                            break;
                        case 14:
                            _l_nestedarr = d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            _r_assignedValid |= ((ushort)1) << 13;
                            break;
                        case 15:
                            _l_intimm = d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            _r_assignedValid |= ((ushort)1) << 14;
                            break;
                        case 16:
                            _l_color = d.GetNextValue<Serde.Test.AllInOne.ColorEnum, AllInOneColorEnumWrap>();
                            _r_assignedValid |= ((ushort)1) << 15;
                            break;
                    }
                }

                if (_r_assignedValid != 0b1111111111111111)
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
}