//HintName: C.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "ColorInt",
            "ColorByte",
            "ColorLong",
            "ColorULong"
        };
        return deserializer.DeserializeType("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";

        private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static readonly FieldNameVisitor Instance = new FieldNameVisitor();
            public static byte Deserialize(IDeserializer deserializer) => deserializer.DeserializeString(Instance);
            public string ExpectedTypeName => "string";

            byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
            public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
            {
                switch (s[0])
                {
                    case (byte)'c'when s.SequenceEqual("colorInt"u8):
                        return 1;
                    case (byte)'c'when s.SequenceEqual("colorByte"u8):
                        return 2;
                    case (byte)'c'when s.SequenceEqual("colorLong"u8):
                        return 3;
                    case (byte)'c'when s.SequenceEqual("colorULong"u8):
                        return 4;
                    default:
                        return 0;
                }
            }
        }

        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            ColorInt _l_colorint = default !;
            ColorByte _l_colorbyte = default !;
            ColorLong _l_colorlong = default !;
            ColorULong _l_colorulong = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_colorint = d.GetNextValue<ColorInt, global::ColorIntWrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 2:
                        _l_colorbyte = d.GetNextValue<ColorByte, global::ColorByteWrap>();
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 3:
                        _l_colorlong = d.GetNextValue<ColorLong, global::ColorLongWrap>();
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 4:
                        _l_colorulong = d.GetNextValue<ColorULong, global::ColorULongWrap>();
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                }
            }

            if (_r_assignedValid != 0b1111)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new C()
            {
                ColorInt = _l_colorint,
                ColorByte = _l_colorbyte,
                ColorLong = _l_colorlong,
                ColorULong = _l_colorulong,
            };
            return newType;
        }
    }
}