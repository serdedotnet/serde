
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
                    Serde.Option<string?> s = default;
                    Serde.Option<System.Collections.Generic.Dictionary<string, string?>> dict = default;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                s = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                break;
                            case 2:
                                dict = d.GetNextValue<System.Collections.Generic.Dictionary<string, string?>, DictWrap.DeserializeImpl<string, StringWrap, string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>>();
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.NullableFields()
                    {
                        S = s.GetValueOrDefault(null),
                        Dict = dict.GetValueOrThrow("Dict"),
                    };
                    return newType;
                }
            }
        }
    }
}