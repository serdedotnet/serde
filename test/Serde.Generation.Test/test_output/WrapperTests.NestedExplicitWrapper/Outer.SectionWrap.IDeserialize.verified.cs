//HintName: Outer.SectionWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class Outer
{
    partial record struct SectionWrap : Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>
    {
        static System.Collections.Specialized.BitVector32.Section Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "Mask",
                "Offset"
            };
            return deserializer.DeserializeType<System.Collections.Specialized.BitVector32.Section, SerdeVisitor>("Section", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>
        {
            public string ExpectedTypeName => "System.Collections.Specialized.BitVector32.Section";

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
                        case (byte)'m'when s.SequenceEqual("mask"u8):
                            return 1;
                        case (byte)'o'when s.SequenceEqual("offset"u8):
                            return 2;
                        default:
                            return 0;
                    }
                }
            }

            System.Collections.Specialized.BitVector32.Section Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>.VisitDictionary<D>(ref D d)
            {
                short _l_mask = default !;
                short _l_offset = default !;
                byte _r_assignedValid = 0b0;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_mask = d.GetNextValue<short, Int16Wrap>();
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case 2:
                            _l_offset = d.GetNextValue<short, Int16Wrap>();
                            _r_assignedValid |= ((byte)1) << 1;
                            break;
                    }
                }

                if (_r_assignedValid != 0b11)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new System.Collections.Specialized.BitVector32.Section()
                {
                    Mask = _l_mask,
                    Offset = _l_offset,
                };
                return newType;
            }
        }
    }
}