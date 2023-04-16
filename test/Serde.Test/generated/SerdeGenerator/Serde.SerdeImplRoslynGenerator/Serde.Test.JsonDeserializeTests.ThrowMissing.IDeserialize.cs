
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct ThrowMissing : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing>
        {
            static Serde.Test.JsonDeserializeTests.ThrowMissing Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "Present",
                    "Missing"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.ThrowMissing, SerdeVisitor>("ThrowMissing", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.ThrowMissing>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.ThrowMissing";
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
                            case (byte)'p'when s.SequenceEqual("present"u8):
                                return 1;
                            case (byte)'m'when s.SequenceEqual("missing"u8):
                                return 2;
                            default:
                                return 0;
                        }
                    }
                }

                Serde.Test.JsonDeserializeTests.ThrowMissing Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.ThrowMissing>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<string> present = default;
                    Serde.Option<string?> missing = default;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                present = d.GetNextValue<string, StringWrap>();
                                break;
                            case 2:
                                missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.ThrowMissing()
                    {
                        Present = present.GetValueOrThrow("Present"),
                        Missing = missing.GetValueOrThrow("Missing"),
                    };
                    return newType;
                }
            }
        }
    }
}