//HintName: Rgb.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "Red",
            "Green",
            "Blue"
        };
        return deserializer.DeserializeType<Rgb, SerdeVisitor>("Rgb", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => "Rgb";

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
                    case (byte)'r'when s.SequenceEqual("red"u8):
                        return 1;
                    case (byte)'g'when s.SequenceEqual("green"u8):
                        return 2;
                    case (byte)'b'when s.SequenceEqual("blue"u8):
                        return 3;
                    default:
                        return 0;
                }
            }
        }

        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> _l_red = default;
            Serde.Option<byte> _l_green = default;
            Serde.Option<byte> _l_blue = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case 2:
                        _l_green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case 3:
                        _l_blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                }
            }

            var newType = new Rgb()
            {
                Red = _l_red.GetValueOrThrow("Red"),
                Green = _l_green.GetValueOrThrow("Green"),
                Blue = _l_blue.GetValueOrThrow("Blue"),
            };
            return newType;
        }
    }
}