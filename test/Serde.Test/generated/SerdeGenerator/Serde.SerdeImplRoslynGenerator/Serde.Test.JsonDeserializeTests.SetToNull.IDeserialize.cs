
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct SetToNull : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>
        {
            static Serde.Test.JsonDeserializeTests.SetToNull Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "Present",
                    "Missing"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.SetToNull, SerdeVisitor>("SetToNull", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.SetToNull>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.SetToNull";
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

                Serde.Test.JsonDeserializeTests.SetToNull Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.SetToNull>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<string> _l_present = default;
                    Serde.Option<string?> _l_missing = default;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                _l_present = d.GetNextValue<string, StringWrap>();
                                break;
                            case 2:
                                _l_missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.SetToNull()
                    {
                        Present = _l_present.GetValueOrThrow("Present"),
                        Missing = _l_missing.GetValueOrDefault(null),
                    };
                    return newType;
                }
            }
        }
    }
}