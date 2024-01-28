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
        return deserializer.DeserializeType("Point", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Point>
    {
        public string ExpectedTypeName => "Point";

        private struct FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static byte Deserialize<D>(ref D deserializer)
                where D : IDeserializer => deserializer.DeserializeString(new FieldNameVisitor());
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
            int _l_x = default !;
            int _l_y = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_x = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 2:
                        _l_y = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                }
            }

            if (_r_assignedValid != 0b11)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new Point()
            {
                X = _l_x,
                Y = _l_y,
            };
            return newType;
        }
    }
}