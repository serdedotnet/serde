//HintName: S.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "One",
            "TwoWord"
        };
        return deserializer.DeserializeType("S", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => "S";

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
                    case (byte)'o'when s.SequenceEqual("one"u8):
                        return 1;
                    case (byte)'t'when s.SequenceEqual("two-word"u8):
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            int _l_one = default !;
            int _l_twoword = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_one = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 2:
                        _l_twoword = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                }
            }

            if (_r_assignedValid != 0b11)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new S()
            {
                One = _l_one,
                TwoWord = _l_twoword,
            };
            return newType;
        }
    }
}