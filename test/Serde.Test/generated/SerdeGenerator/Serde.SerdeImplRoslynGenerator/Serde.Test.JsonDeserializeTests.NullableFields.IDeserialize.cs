
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class NullableFields : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>
        {
            static Serde.Test.JsonDeserializeTests.NullableFields Serde.IDeserialize<Serde.Test.JsonDeserializeTests.NullableFields>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "S",
                    "Dict"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.NullableFields, SerdeVisitor>("NullableFields", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.NullableFields>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.NullableFields";

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
                            case (byte)'s'when s.SequenceEqual("s"u8):
                                return 1;
                            case (byte)'d'when s.SequenceEqual("dict"u8):
                                return 2;
                            default:
                                return 0;
                        }
                    }
                }

                Serde.Test.JsonDeserializeTests.NullableFields Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.NullableFields>.VisitDictionary<D>(ref D d)
                {
                    string? _l_s = default !;
                    System.Collections.Generic.Dictionary<string, string?> _l_dict = default !;
                    byte _r_assignedValid = 0b1;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                _l_s = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 2:
                                _l_dict = d.GetNextValue<System.Collections.Generic.Dictionary<string, string?>, DictWrap.DeserializeImpl<string, StringWrap, string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>>();
                                _r_assignedValid |= ((byte)1) << 1;
                                break;
                        }
                    }

                    if (_r_assignedValid != 0b11)
                    {
                        throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.NullableFields()
                    {
                        S = _l_s,
                        Dict = _l_dict,
                    };
                    return newType;
                }
            }
        }
    }
}