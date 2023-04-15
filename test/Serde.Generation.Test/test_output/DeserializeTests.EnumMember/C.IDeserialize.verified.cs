//HintName: C.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "ColorInt",
            "ColorByte",
            "ColorLong",
            "ColorULong"
        };
        return deserializer.DeserializeType<C, SerdeVisitor>("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";

        private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static byte Deserialize<D>(ref D deserializer)
                where D : IDeserializer => deserializer.DeserializeString<byte, FieldNameVisitor>(new FieldNameVisitor());
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
            Serde.Option<ColorInt> colorint = default;
            Serde.Option<ColorByte> colorbyte = default;
            Serde.Option<ColorLong> colorlong = default;
            Serde.Option<ColorULong> colorulong = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        colorint = d.GetNextValue<ColorInt, ColorIntWrap>();
                        break;
                    case 2:
                        colorbyte = d.GetNextValue<ColorByte, ColorByteWrap>();
                        break;
                    case 3:
                        colorlong = d.GetNextValue<ColorLong, ColorLongWrap>();
                        break;
                    case 4:
                        colorulong = d.GetNextValue<ColorULong, ColorULongWrap>();
                        break;
                }
            }

            var newType = new C()
            {
                ColorInt = colorint.GetValueOrThrow("ColorInt"),
                ColorByte = colorbyte.GetValueOrThrow("ColorByte"),
                ColorLong = colorlong.GetValueOrThrow("ColorLong"),
                ColorULong = colorulong.GetValueOrThrow("ColorULong"),
            };
            return newType;
        }
    }
}