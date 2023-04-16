
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomImArrayExplicitWrapOnMember : Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
        {
            static Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "A"
                };
                return deserializer.DeserializeType<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember, SerdeVisitor>("CustomImArrayExplicitWrapOnMember", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
            {
                public string ExpectedTypeName => "Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember";
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
                            case (byte)'a'when s.SequenceEqual("a"u8):
                                return 1;
                            default:
                                return 0;
                        }
                    }
                }

                Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember Serde.IDeserializeVisitor<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<Serde.Test.GenericWrapperTests.CustomImArray<int>> _l_a = default;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                _l_a = d.GetNextValue<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                                break;
                        }
                    }

                    var newType = new Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember()
                    {
                        A = _l_a.GetValueOrThrow("A"),
                    };
                    return newType;
                }
            }
        }
    }
}