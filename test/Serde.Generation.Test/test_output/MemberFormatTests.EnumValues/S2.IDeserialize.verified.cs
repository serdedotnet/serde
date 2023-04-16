//HintName: S2.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S2 : Serde.IDeserialize<S2>
{
    static S2 Serde.IDeserialize<S2>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "E"
        };
        return deserializer.DeserializeType<S2, SerdeVisitor>("S2", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S2>
    {
        public string ExpectedTypeName => "S2";

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
                    case (byte)'E'when s.SequenceEqual("E"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        S2 Serde.IDeserializeVisitor<S2>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<ColorEnum> e = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        e = d.GetNextValue<ColorEnum, ColorEnumWrap>();
                        break;
                }
            }

            var newType = new S2()
            {
                E = e.GetValueOrThrow("E"),
            };
            return newType;
        }
    }
}