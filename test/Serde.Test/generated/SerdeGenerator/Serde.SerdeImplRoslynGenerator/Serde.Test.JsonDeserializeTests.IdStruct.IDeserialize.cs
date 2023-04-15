﻿
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial struct IdStruct : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStruct>
        {
            static Serde.Test.JsonDeserializeTests.IdStruct Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStruct>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "Id"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.IdStruct, SerdeVisitor>("IdStruct", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.IdStruct>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.IdStruct";
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
                            case (byte)'i'when s.SequenceEqual("id"u8):
                                return 1;
                            default:
                                return 0;
                        }
                    }
                }

                Serde.Test.JsonDeserializeTests.IdStruct Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.IdStruct>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<int> id = default;
                    while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 1:
                                id = d.GetNextValue<int, Int32Wrap>();
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.IdStruct()
                    {
                        Id = id.GetValueOrThrow("Id"),
                    };
                    return newType;
                }
            }
        }
    }
}