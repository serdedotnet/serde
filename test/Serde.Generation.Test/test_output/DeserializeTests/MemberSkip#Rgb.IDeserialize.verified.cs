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
            "Blue"
        };
        return deserializer.DeserializeType<Rgb, SerdeVisitor>("Rgb", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => "Rgb";

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
                    case (byte)'r'when s.SequenceEqual("red"u8):
                        return 1;
                    case (byte)'b'when s.SequenceEqual("blue"u8):
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> red = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case 2:
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                }
            }

            var newType = new Rgb()
            {
                Red = red.GetValueOrThrow("Red"),
                Blue = blue.GetValueOrThrow("Blue"),
            };
            return newType;
        }
    }
}