//HintName: Serde.Test.AllInOne.IDeserialize.cs

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
                Serde.Option<bool> _l_boolfield = default;
                Serde.Option<char> _l_charfield = default;
                Serde.Option<byte> _l_bytefield = default;
                Serde.Option<ushort> _l_ushortfield = default;
                Serde.Option<uint> _l_uintfield = default;
                Serde.Option<ulong> _l_ulongfield = default;
                Serde.Option<sbyte> _l_sbytefield = default;
                Serde.Option<short> _l_shortfield = default;
                Serde.Option<int> _l_intfield = default;
                Serde.Option<long> _l_longfield = default;
                Serde.Option<string> _l_stringfield = default;
                Serde.Option<string?> _l_nullstringfield = default;
                Serde.Option<uint[]> _l_uintarr = default;
                Serde.Option<int[][]> _l_nestedarr = default;
                Serde.Option<System.Collections.Immutable.ImmutableArray<int>> _l_intimm = default;
                Serde.Option<Serde.Test.AllInOne.ColorEnum> _l_color = default;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_boolfield = d.GetNextValue<bool, BoolWrap>();
                            break;
                        case 2:
                            _l_charfield = d.GetNextValue<char, CharWrap>();
                            break;
                        case 3:
                            _l_bytefield = d.GetNextValue<byte, ByteWrap>();
                            break;
                        case 4:
                            _l_ushortfield = d.GetNextValue<ushort, UInt16Wrap>();
                            break;
                        case 5:
                            _l_uintfield = d.GetNextValue<uint, UInt32Wrap>();
                            break;
                        case 6:
                            _l_ulongfield = d.GetNextValue<ulong, UInt64Wrap>();
                            break;
                        case 7:
                            _l_sbytefield = d.GetNextValue<sbyte, SByteWrap>();
                            break;
                        case 8:
                            _l_shortfield = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case 9:
                            _l_intfield = d.GetNextValue<int, Int32Wrap>();
                            break;
                        case 10:
                            _l_longfield = d.GetNextValue<long, Int64Wrap>();
                            break;
                        case 11:
                            _l_stringfield = d.GetNextValue<string, StringWrap>();
                            break;
                        case 12:
                            _l_nullstringfield = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                            break;
                        case 13:
                            _l_uintarr = d.GetNextValue<uint[], ArrayWrap.DeserializeImpl<uint, UInt32Wrap>>();
                            break;
                        case 14:
                            _l_nestedarr = d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case 15:
                            _l_intimm = d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case 16:
                            _l_color = d.GetNextValue<Serde.Test.AllInOne.ColorEnum, AllInOneColorEnumWrap>();
                            break;
                    }
                }

                var newType = new Serde.Test.AllInOne()
                {
                    BoolField = _l_boolfield.GetValueOrThrow("BoolField"),
                    CharField = _l_charfield.GetValueOrThrow("CharField"),
                    ByteField = _l_bytefield.GetValueOrThrow("ByteField"),
                    UShortField = _l_ushortfield.GetValueOrThrow("UShortField"),
                    UIntField = _l_uintfield.GetValueOrThrow("UIntField"),
                    ULongField = _l_ulongfield.GetValueOrThrow("ULongField"),
                    SByteField = _l_sbytefield.GetValueOrThrow("SByteField"),
                    ShortField = _l_shortfield.GetValueOrThrow("ShortField"),
                    IntField = _l_intfield.GetValueOrThrow("IntField"),
                    LongField = _l_longfield.GetValueOrThrow("LongField"),
                    StringField = _l_stringfield.GetValueOrThrow("StringField"),
                    NullStringField = _l_nullstringfield.GetValueOrDefault(null),
                    UIntArr = _l_uintarr.GetValueOrThrow("UIntArr"),
                    NestedArr = _l_nestedarr.GetValueOrThrow("NestedArr"),
                    IntImm = _l_intimm.GetValueOrThrow("IntImm"),
                    Color = _l_color.GetValueOrThrow("Color"),
                };
                return newType;
            }
        }
    }
}