//HintName: Outer.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct Outer : Serde.IDeserialize<Outer>
{
    static Outer Serde.IDeserialize<Outer>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "Value"
        };
        return deserializer.DeserializeType<Outer, SerdeVisitor>("Outer", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Outer>
    {
        public string ExpectedTypeName => "Outer";

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
                    case (byte)'v'when s.SequenceEqual("value"u8):
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        Outer Serde.IDeserializeVisitor<Outer>.VisitDictionary<D>(ref D d)
        {
            StringWrapper _l_value = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_value = d.GetNextValue<StringWrapper, StringWrapper>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                }
            }

            if (_r_assignedValid != 0b1)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new Outer()
            {
                Value = _l_value,
            };
            return newType;
        }
    }
}