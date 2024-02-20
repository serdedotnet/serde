//HintName: Test.RecursiveWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct RecursiveWrap : Serde.IDeserialize<Recursive>
    {
        static Recursive Serde.IDeserialize<Recursive>.Deserialize(IDeserializer deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "Next"
            };
            return deserializer.DeserializeType("Recursive", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Recursive>
        {
            public string ExpectedTypeName => "Recursive";

            private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
            {
                public static readonly FieldNameVisitor Instance = new FieldNameVisitor();
                public static byte Deserialize(IDeserializer deserializer) => deserializer.DeserializeString(Instance);
                public string ExpectedTypeName => "string";

                byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
                public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
                {
                    switch (s[0])
                    {
                        case (byte)'n'when s.SequenceEqual("next"u8):
                            return 1;
                        default:
                            return 0;
                    }
                }
            }

            Recursive Serde.IDeserializeVisitor<Recursive>.VisitDictionary<D>(ref D d)
            {
                Recursive? _l_next = default !;
                byte _r_assignedValid = 0b1;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_next = d.GetNextValue<Recursive?, RecursiveWrap>();
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                    }
                }

                if (_r_assignedValid != 0b1)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Recursive()
                {
                    Next = _l_next,
                };
                return newType;
            }
        }
    }
}