//HintName: R.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record R : Serde.IDeserialize<R>
{
    static R Serde.IDeserialize<R>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "A",
            "B"
        };
        return deserializer.DeserializeType<R, SerdeVisitor>("R", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<R>
    {
        public string ExpectedTypeName => "R";

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
                    case (byte)'a'when s.SequenceEqual("a"u8):
                        return 1;
                    case (byte)'b'when s.SequenceEqual("b"u8):
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        R Serde.IDeserializeVisitor<R>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> _l_a = default;
            Serde.Option<string> _l_b = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_a = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case 2:
                        _l_b = d.GetNextValue<string, StringWrap>();
                        break;
                }
            }

            var newType = new R(_l_a.GetValueOrThrow("A"), _l_b.GetValueOrThrow("B"))
            {
            };
            return newType;
        }
    }
}