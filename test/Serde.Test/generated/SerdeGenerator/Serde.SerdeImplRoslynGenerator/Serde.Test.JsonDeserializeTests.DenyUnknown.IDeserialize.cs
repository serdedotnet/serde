
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct DenyUnknown : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DenyUnknown>
        {
            static Serde.Test.JsonDeserializeTests.DenyUnknown Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DenyUnknown>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "Present",
                    "Missing"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.DenyUnknown, SerdeVisitor>("DenyUnknown", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.DenyUnknown>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.DenyUnknown";

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
                                throw new InvalidDeserializeValueException("Unexpected field or property name in type Serde.Test.JsonDeserializeTests.DenyUnknown: '" + System.Text.Encoding.UTF8.GetString(s) + "'");
                        }
                    }
                }

                Serde.Test.JsonDeserializeTests.DenyUnknown Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.DenyUnknown>.VisitDictionary<D>(ref D d)
                {
                    string _l_present = default !;
                    string? _l_missing = default !;
                    byte _r_assignedValid = 0b10;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                _l_present = d.GetNextValue<string, StringWrap>();
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 2:
                                _l_missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                _r_assignedValid |= ((byte)1) << 1;
                                break;
                        }
                    }

                    if (_r_assignedValid != 0b11)
                    {
                        throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.DenyUnknown()
                    {
                        Present = _l_present,
                        Missing = _l_missing,
                    };
                    return newType;
                }
            }
        }
    }
}