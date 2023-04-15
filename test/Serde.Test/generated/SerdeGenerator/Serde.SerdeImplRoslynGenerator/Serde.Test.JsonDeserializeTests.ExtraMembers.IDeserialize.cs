
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial struct ExtraMembers : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ExtraMembers>
        {
            static Serde.Test.JsonDeserializeTests.ExtraMembers Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ExtraMembers>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "b"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.ExtraMembers, SerdeVisitor>("ExtraMembers", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.ExtraMembers>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.ExtraMembers";
                private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
                {
                    public static byte Deserialize<D>(ref D deserializer)
                        where D : IDeserializer => deserializer.DeserializeString<byte, FieldNameVisitor>(new FieldNameVisitor());
                    public string ExpectedTypeName => "string";
                    byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
                    public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
                    {
                        switch (s[0])
                        {
                            case (byte)'b'when s.SequenceEqual("b"u8):
                                return 1;
                            default:
                                return 0;
                        }
                    }
                }

                Serde.Test.JsonDeserializeTests.ExtraMembers Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.ExtraMembers>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<int> b = default;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                b = d.GetNextValue<int, Int32Wrap>();
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.ExtraMembers()
                    {
                        b = b.GetValueOrThrow("b"),
                    };
                    return newType;
                }
            }
        }
    }
}