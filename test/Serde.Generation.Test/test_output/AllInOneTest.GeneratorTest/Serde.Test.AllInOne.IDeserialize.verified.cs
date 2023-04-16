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
                Serde.Option<bool> boolfield = default;
                Serde.Option<char> charfield = default;
                Serde.Option<byte> bytefield = default;
                Serde.Option<ushort> ushortfield = default;
                Serde.Option<uint> uintfield = default;
                Serde.Option<ulong> ulongfield = default;
                Serde.Option<sbyte> sbytefield = default;
                Serde.Option<short> shortfield = default;
                Serde.Option<int> intfield = default;
                Serde.Option<long> longfield = default;
                Serde.Option<string> stringfield = default;
                Serde.Option<string?> nullstringfield = default;
                Serde.Option<uint[]> uintarr = default;
                Serde.Option<int[][]> nestedarr = default;
                Serde.Option<System.Collections.Immutable.ImmutableArray<int>> intimm = default;
                Serde.Option<Serde.Test.AllInOne.ColorEnum> color = default;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            boolfield = d.GetNextValue<bool, BoolWrap>();
                            break;
                        case 2:
                            charfield = d.GetNextValue<char, CharWrap>();
                            break;
                        case 3:
                            bytefield = d.GetNextValue<byte, ByteWrap>();
                            break;
                        case 4:
                            ushortfield = d.GetNextValue<ushort, UInt16Wrap>();
                            break;
                        case 5:
                            uintfield = d.GetNextValue<uint, UInt32Wrap>();
                            break;
                        case 6:
                            ulongfield = d.GetNextValue<ulong, UInt64Wrap>();
                            break;
                        case 7:
                            sbytefield = d.GetNextValue<sbyte, SByteWrap>();
                            break;
                        case 8:
                            shortfield = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case 9:
                            intfield = d.GetNextValue<int, Int32Wrap>();
                            break;
                        case 10:
                            longfield = d.GetNextValue<long, Int64Wrap>();
                            break;
                        case 11:
                            stringfield = d.GetNextValue<string, StringWrap>();
                            break;
                        case 12:
                            nullstringfield = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                            break;
                        case 13:
                            uintarr = d.GetNextValue<uint[], ArrayWrap.DeserializeImpl<uint, UInt32Wrap>>();
                            break;
                        case 14:
                            nestedarr = d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case 15:
                            intimm = d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case 16:
                            color = d.GetNextValue<Serde.Test.AllInOne.ColorEnum, AllInOneColorEnumWrap>();
                            break;
                    }
                }

                var newType = new Serde.Test.AllInOne()
                {
                    BoolField = boolfield.GetValueOrThrow("BoolField"),
                    CharField = charfield.GetValueOrThrow("CharField"),
                    ByteField = bytefield.GetValueOrThrow("ByteField"),
                    UShortField = ushortfield.GetValueOrThrow("UShortField"),
                    UIntField = uintfield.GetValueOrThrow("UIntField"),
                    ULongField = ulongfield.GetValueOrThrow("ULongField"),
                    SByteField = sbytefield.GetValueOrThrow("SByteField"),
                    ShortField = shortfield.GetValueOrThrow("ShortField"),
                    IntField = intfield.GetValueOrThrow("IntField"),
                    LongField = longfield.GetValueOrThrow("LongField"),
                    StringField = stringfield.GetValueOrThrow("StringField"),
                    NullStringField = nullstringfield.GetValueOrDefault(null),
                    UIntArr = uintarr.GetValueOrThrow("UIntArr"),
                    NestedArr = nestedarr.GetValueOrThrow("NestedArr"),
                    IntImm = intimm.GetValueOrThrow("IntImm"),
                    Color = color.GetValueOrThrow("Color"),
                };
                return newType;
            }
        }
    }
}