//HintName: Serde.BitVector32SectionWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>
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
                Serde.Option<short> mask = default;
                Serde.Option<short> offset = default;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            mask = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case 2:
                            offset = d.GetNextValue<short, Int16Wrap>();
                            break;
                    }
                }

                var newType = new System.Collections.Specialized.BitVector32.Section()
                {
                    Mask = mask.GetValueOrThrow("Mask"),
                    Offset = offset.GetValueOrThrow("Offset"),
                };
                return newType;
            }
        }
    }
}