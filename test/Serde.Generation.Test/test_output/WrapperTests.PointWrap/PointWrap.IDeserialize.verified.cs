//HintName: PointWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct PointWrap : Serde.IDeserialize<Point>
{
    static Point Serde.IDeserialize<Point>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "X",
            "Y"
        };
        return deserializer.DeserializeType<Point, SerdeVisitor>("Point", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Point>
    {
        public string ExpectedTypeName => "Point";

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
                    case (byte)'x'when s.SequenceEqual("x"u8):
                        return 1;
                    case (byte)'y'when s.SequenceEqual("y"u8):
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        Point Serde.IDeserializeVisitor<Point>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> x = default;
            Serde.Option<int> y = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        x = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case 2:
                        y = d.GetNextValue<int, Int32Wrap>();
                        break;
                }
            }

            var newType = new Point()
            {
                X = x.GetValueOrThrow("X"),
                Y = y.GetValueOrThrow("Y"),
            };
            return newType;
        }
    }
}