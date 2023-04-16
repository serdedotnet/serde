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
            "S"
        };
        return deserializer.DeserializeType<C, SerdeVisitor>("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";

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
                    case (byte)'s'when s.SequenceEqual("s"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<System.Collections.Specialized.BitVector32.Section> _l_s = default;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_s = d.GetNextValue<System.Collections.Specialized.BitVector32.Section, BitVector32SectionWrap>();
                        break;
                }
            }

            var newType = new C()
            {
                S = _l_s.GetValueOrThrow("S"),
            };
            return newType;
        }
    }
}