
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial struct MaxSizeType : Serde.IDeserialize<Serde.Test.MaxSizeType>
    {
        static Serde.Test.MaxSizeType Serde.IDeserialize<Serde.Test.MaxSizeType>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "Field1",
                "Field2",
                "Field3",
                "Field4",
                "Field5",
                "Field6",
                "Field7",
                "Field8",
                "Field9",
                "Field10",
                "Field11",
                "Field12",
                "Field13",
                "Field14",
                "Field15",
                "Field16",
                "Field17",
                "Field18",
                "Field19",
                "Field20",
                "Field21",
                "Field22",
                "Field23",
                "Field24",
                "Field25",
                "Field26",
                "Field27",
                "Field28",
                "Field29",
                "Field30",
                "Field31",
                "Field32",
                "Field33",
                "Field34",
                "Field35",
                "Field36",
                "Field37",
                "Field38",
                "Field39",
                "Field40",
                "Field41",
                "Field42",
                "Field43",
                "Field44",
                "Field45",
                "Field46",
                "Field47",
                "Field48",
                "Field49",
                "Field50",
                "Field51",
                "Field52",
                "Field53",
                "Field54",
                "Field55",
                "Field56",
                "Field57",
                "Field58",
                "Field59",
                "Field60",
                "Field61",
                "Field62",
                "Field63",
                "Field64"
            };
            return deserializer.DeserializeType<Serde.Test.MaxSizeType, SerdeVisitor>("MaxSizeType", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.MaxSizeType>
        {
            public string ExpectedTypeName => "Serde.Test.MaxSizeType";

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
                        case (byte)'f'when s.SequenceEqual("field1"u8):
                            return 1;
                        case (byte)'f'when s.SequenceEqual("field2"u8):
                            return 2;
                        case (byte)'f'when s.SequenceEqual("field3"u8):
                            return 3;
                        case (byte)'f'when s.SequenceEqual("field4"u8):
                            return 4;
                        case (byte)'f'when s.SequenceEqual("field5"u8):
                            return 5;
                        case (byte)'f'when s.SequenceEqual("field6"u8):
                            return 6;
                        case (byte)'f'when s.SequenceEqual("field7"u8):
                            return 7;
                        case (byte)'f'when s.SequenceEqual("field8"u8):
                            return 8;
                        case (byte)'f'when s.SequenceEqual("field9"u8):
                            return 9;
                        case (byte)'f'when s.SequenceEqual("field10"u8):
                            return 10;
                        case (byte)'f'when s.SequenceEqual("field11"u8):
                            return 11;
                        case (byte)'f'when s.SequenceEqual("field12"u8):
                            return 12;
                        case (byte)'f'when s.SequenceEqual("field13"u8):
                            return 13;
                        case (byte)'f'when s.SequenceEqual("field14"u8):
                            return 14;
                        case (byte)'f'when s.SequenceEqual("field15"u8):
                            return 15;
                        case (byte)'f'when s.SequenceEqual("field16"u8):
                            return 16;
                        case (byte)'f'when s.SequenceEqual("field17"u8):
                            return 17;
                        case (byte)'f'when s.SequenceEqual("field18"u8):
                            return 18;
                        case (byte)'f'when s.SequenceEqual("field19"u8):
                            return 19;
                        case (byte)'f'when s.SequenceEqual("field20"u8):
                            return 20;
                        case (byte)'f'when s.SequenceEqual("field21"u8):
                            return 21;
                        case (byte)'f'when s.SequenceEqual("field22"u8):
                            return 22;
                        case (byte)'f'when s.SequenceEqual("field23"u8):
                            return 23;
                        case (byte)'f'when s.SequenceEqual("field24"u8):
                            return 24;
                        case (byte)'f'when s.SequenceEqual("field25"u8):
                            return 25;
                        case (byte)'f'when s.SequenceEqual("field26"u8):
                            return 26;
                        case (byte)'f'when s.SequenceEqual("field27"u8):
                            return 27;
                        case (byte)'f'when s.SequenceEqual("field28"u8):
                            return 28;
                        case (byte)'f'when s.SequenceEqual("field29"u8):
                            return 29;
                        case (byte)'f'when s.SequenceEqual("field30"u8):
                            return 30;
                        case (byte)'f'when s.SequenceEqual("field31"u8):
                            return 31;
                        case (byte)'f'when s.SequenceEqual("field32"u8):
                            return 32;
                        case (byte)'f'when s.SequenceEqual("field33"u8):
                            return 33;
                        case (byte)'f'when s.SequenceEqual("field34"u8):
                            return 34;
                        case (byte)'f'when s.SequenceEqual("field35"u8):
                            return 35;
                        case (byte)'f'when s.SequenceEqual("field36"u8):
                            return 36;
                        case (byte)'f'when s.SequenceEqual("field37"u8):
                            return 37;
                        case (byte)'f'when s.SequenceEqual("field38"u8):
                            return 38;
                        case (byte)'f'when s.SequenceEqual("field39"u8):
                            return 39;
                        case (byte)'f'when s.SequenceEqual("field40"u8):
                            return 40;
                        case (byte)'f'when s.SequenceEqual("field41"u8):
                            return 41;
                        case (byte)'f'when s.SequenceEqual("field42"u8):
                            return 42;
                        case (byte)'f'when s.SequenceEqual("field43"u8):
                            return 43;
                        case (byte)'f'when s.SequenceEqual("field44"u8):
                            return 44;
                        case (byte)'f'when s.SequenceEqual("field45"u8):
                            return 45;
                        case (byte)'f'when s.SequenceEqual("field46"u8):
                            return 46;
                        case (byte)'f'when s.SequenceEqual("field47"u8):
                            return 47;
                        case (byte)'f'when s.SequenceEqual("field48"u8):
                            return 48;
                        case (byte)'f'when s.SequenceEqual("field49"u8):
                            return 49;
                        case (byte)'f'when s.SequenceEqual("field50"u8):
                            return 50;
                        case (byte)'f'when s.SequenceEqual("field51"u8):
                            return 51;
                        case (byte)'f'when s.SequenceEqual("field52"u8):
                            return 52;
                        case (byte)'f'when s.SequenceEqual("field53"u8):
                            return 53;
                        case (byte)'f'when s.SequenceEqual("field54"u8):
                            return 54;
                        case (byte)'f'when s.SequenceEqual("field55"u8):
                            return 55;
                        case (byte)'f'when s.SequenceEqual("field56"u8):
                            return 56;
                        case (byte)'f'when s.SequenceEqual("field57"u8):
                            return 57;
                        case (byte)'f'when s.SequenceEqual("field58"u8):
                            return 58;
                        case (byte)'f'when s.SequenceEqual("field59"u8):
                            return 59;
                        case (byte)'f'when s.SequenceEqual("field60"u8):
                            return 60;
                        case (byte)'f'when s.SequenceEqual("field61"u8):
                            return 61;
                        case (byte)'f'when s.SequenceEqual("field62"u8):
                            return 62;
                        case (byte)'f'when s.SequenceEqual("field63"u8):
                            return 63;
                        case (byte)'f'when s.SequenceEqual("field64"u8):
                            return 64;
                        default:
                            return 0;
                    }
                }
            }

            Serde.Test.MaxSizeType Serde.IDeserializeVisitor<Serde.Test.MaxSizeType>.VisitDictionary<D>(ref D d)
            {
                byte _l_field1 = default !;
                byte _l_field2 = default !;
                byte _l_field3 = default !;
                byte _l_field4 = default !;
                byte _l_field5 = default !;
                byte _l_field6 = default !;
                byte _l_field7 = default !;
                byte _l_field8 = default !;
                byte _l_field9 = default !;
                byte _l_field10 = default !;
                byte _l_field11 = default !;
                byte _l_field12 = default !;
                byte _l_field13 = default !;
                byte _l_field14 = default !;
                byte _l_field15 = default !;
                byte _l_field16 = default !;
                byte _l_field17 = default !;
                byte _l_field18 = default !;
                byte _l_field19 = default !;
                byte _l_field20 = default !;
                byte _l_field21 = default !;
                byte _l_field22 = default !;
                byte _l_field23 = default !;
                byte _l_field24 = default !;
                byte _l_field25 = default !;
                byte _l_field26 = default !;
                byte _l_field27 = default !;
                byte _l_field28 = default !;
                byte _l_field29 = default !;
                byte _l_field30 = default !;
                byte _l_field31 = default !;
                byte _l_field32 = default !;
                byte _l_field33 = default !;
                byte _l_field34 = default !;
                byte _l_field35 = default !;
                byte _l_field36 = default !;
                byte _l_field37 = default !;
                byte _l_field38 = default !;
                byte _l_field39 = default !;
                byte _l_field40 = default !;
                byte _l_field41 = default !;
                byte _l_field42 = default !;
                byte _l_field43 = default !;
                byte _l_field44 = default !;
                byte _l_field45 = default !;
                byte _l_field46 = default !;
                byte _l_field47 = default !;
                byte _l_field48 = default !;
                byte _l_field49 = default !;
                byte _l_field50 = default !;
                byte _l_field51 = default !;
                byte _l_field52 = default !;
                byte _l_field53 = default !;
                byte _l_field54 = default !;
                byte _l_field55 = default !;
                byte _l_field56 = default !;
                byte _l_field57 = default !;
                byte _l_field58 = default !;
                byte _l_field59 = default !;
                byte _l_field60 = default !;
                byte _l_field61 = default !;
                byte _l_field62 = default !;
                byte _l_field63 = default !;
                byte _l_field64 = default !;
                ulong _r_assignedValid = 0b0;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_field1 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 0;
                            break;
                        case 2:
                            _l_field2 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 1;
                            break;
                        case 3:
                            _l_field3 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 2;
                            break;
                        case 4:
                            _l_field4 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 3;
                            break;
                        case 5:
                            _l_field5 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 4;
                            break;
                        case 6:
                            _l_field6 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 5;
                            break;
                        case 7:
                            _l_field7 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 6;
                            break;
                        case 8:
                            _l_field8 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 7;
                            break;
                        case 9:
                            _l_field9 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 8;
                            break;
                        case 10:
                            _l_field10 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 9;
                            break;
                        case 11:
                            _l_field11 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 10;
                            break;
                        case 12:
                            _l_field12 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 11;
                            break;
                        case 13:
                            _l_field13 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 12;
                            break;
                        case 14:
                            _l_field14 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 13;
                            break;
                        case 15:
                            _l_field15 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 14;
                            break;
                        case 16:
                            _l_field16 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 15;
                            break;
                        case 17:
                            _l_field17 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 16;
                            break;
                        case 18:
                            _l_field18 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 17;
                            break;
                        case 19:
                            _l_field19 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 18;
                            break;
                        case 20:
                            _l_field20 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 19;
                            break;
                        case 21:
                            _l_field21 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 20;
                            break;
                        case 22:
                            _l_field22 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 21;
                            break;
                        case 23:
                            _l_field23 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 22;
                            break;
                        case 24:
                            _l_field24 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 23;
                            break;
                        case 25:
                            _l_field25 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 24;
                            break;
                        case 26:
                            _l_field26 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 25;
                            break;
                        case 27:
                            _l_field27 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 26;
                            break;
                        case 28:
                            _l_field28 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 27;
                            break;
                        case 29:
                            _l_field29 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 28;
                            break;
                        case 30:
                            _l_field30 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 29;
                            break;
                        case 31:
                            _l_field31 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 30;
                            break;
                        case 32:
                            _l_field32 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 31;
                            break;
                        case 33:
                            _l_field33 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 32;
                            break;
                        case 34:
                            _l_field34 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 33;
                            break;
                        case 35:
                            _l_field35 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 34;
                            break;
                        case 36:
                            _l_field36 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 35;
                            break;
                        case 37:
                            _l_field37 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 36;
                            break;
                        case 38:
                            _l_field38 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 37;
                            break;
                        case 39:
                            _l_field39 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 38;
                            break;
                        case 40:
                            _l_field40 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 39;
                            break;
                        case 41:
                            _l_field41 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 40;
                            break;
                        case 42:
                            _l_field42 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 41;
                            break;
                        case 43:
                            _l_field43 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 42;
                            break;
                        case 44:
                            _l_field44 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 43;
                            break;
                        case 45:
                            _l_field45 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 44;
                            break;
                        case 46:
                            _l_field46 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 45;
                            break;
                        case 47:
                            _l_field47 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 46;
                            break;
                        case 48:
                            _l_field48 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 47;
                            break;
                        case 49:
                            _l_field49 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 48;
                            break;
                        case 50:
                            _l_field50 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 49;
                            break;
                        case 51:
                            _l_field51 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 50;
                            break;
                        case 52:
                            _l_field52 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 51;
                            break;
                        case 53:
                            _l_field53 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 52;
                            break;
                        case 54:
                            _l_field54 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 53;
                            break;
                        case 55:
                            _l_field55 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 54;
                            break;
                        case 56:
                            _l_field56 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 55;
                            break;
                        case 57:
                            _l_field57 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 56;
                            break;
                        case 58:
                            _l_field58 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 57;
                            break;
                        case 59:
                            _l_field59 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 58;
                            break;
                        case 60:
                            _l_field60 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 59;
                            break;
                        case 61:
                            _l_field61 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 60;
                            break;
                        case 62:
                            _l_field62 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 61;
                            break;
                        case 63:
                            _l_field63 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 62;
                            break;
                        case 64:
                            _l_field64 = d.GetNextValue<byte, ByteWrap>();
                            _r_assignedValid |= ((ulong)1) << 63;
                            break;
                    }
                }

                if (_r_assignedValid != 0b1111111111111111111111111111111111111111111111111111111111111111)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Serde.Test.MaxSizeType()
                {
                    Field1 = _l_field1,
                    Field2 = _l_field2,
                    Field3 = _l_field3,
                    Field4 = _l_field4,
                    Field5 = _l_field5,
                    Field6 = _l_field6,
                    Field7 = _l_field7,
                    Field8 = _l_field8,
                    Field9 = _l_field9,
                    Field10 = _l_field10,
                    Field11 = _l_field11,
                    Field12 = _l_field12,
                    Field13 = _l_field13,
                    Field14 = _l_field14,
                    Field15 = _l_field15,
                    Field16 = _l_field16,
                    Field17 = _l_field17,
                    Field18 = _l_field18,
                    Field19 = _l_field19,
                    Field20 = _l_field20,
                    Field21 = _l_field21,
                    Field22 = _l_field22,
                    Field23 = _l_field23,
                    Field24 = _l_field24,
                    Field25 = _l_field25,
                    Field26 = _l_field26,
                    Field27 = _l_field27,
                    Field28 = _l_field28,
                    Field29 = _l_field29,
                    Field30 = _l_field30,
                    Field31 = _l_field31,
                    Field32 = _l_field32,
                    Field33 = _l_field33,
                    Field34 = _l_field34,
                    Field35 = _l_field35,
                    Field36 = _l_field36,
                    Field37 = _l_field37,
                    Field38 = _l_field38,
                    Field39 = _l_field39,
                    Field40 = _l_field40,
                    Field41 = _l_field41,
                    Field42 = _l_field42,
                    Field43 = _l_field43,
                    Field44 = _l_field44,
                    Field45 = _l_field45,
                    Field46 = _l_field46,
                    Field47 = _l_field47,
                    Field48 = _l_field48,
                    Field49 = _l_field49,
                    Field50 = _l_field50,
                    Field51 = _l_field51,
                    Field52 = _l_field52,
                    Field53 = _l_field53,
                    Field54 = _l_field54,
                    Field55 = _l_field55,
                    Field56 = _l_field56,
                    Field57 = _l_field57,
                    Field58 = _l_field58,
                    Field59 = _l_field59,
                    Field60 = _l_field60,
                    Field61 = _l_field61,
                    Field62 = _l_field62,
                    Field63 = _l_field63,
                    Field64 = _l_field64,
                };
                return newType;
            }
        }
    }
}