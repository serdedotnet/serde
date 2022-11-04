
using System;
using System.Collections.Generic;
using Serde.Json;
using Xunit;
using static Serde.Json.JsonValue;
using Array = Serde.Json.JsonValue.Array;

namespace Serde.Test
{
    public partial class JsonDeserializeTests
    {
        [Fact]
        public void DeserializeEnumerable()
        {
            var src = @"[ 1, 2 ]";
            var result = JsonSerializer.Deserialize<JsonValue>(src);
            Assert.Equal(new Array(new JsonValue[] {
                new Number(1),
                new Number(2)
            }), result);
        }

        [Fact]
        public void NestedEnumerable()
        {
            var src = @"[
                [ 1, ""abc"" ],
                [ 2 ],
                [ ""def"", 3 ],
                []
            ]";
            var result = JsonSerializer.Deserialize<JsonValue>(src);
            Assert.Equal(new Array(new JsonValue[] {
                new Array(new JsonValue[] { 1, "abc" }),
                new Array(new JsonValue[] { 2 }),
                new Array(new JsonValue[] { "def", 3 }),
                Array.Empty
            }), result);
        }

        [Fact]
        public void DeserializeObject()
        {
            var src = @"{ ""field1"": ""abc"", ""field2"": 123 }";
            var result = JsonSerializer.Deserialize<JsonValue>(src);
            Assert.Equal(new JsonValue.Object(new (string, JsonValue)[] {
                ("field1", "abc"),
                ("field2", 123)
            }), result);
        }

        [Fact]
        public void DeserializeInt()
        {
            var src = "123";
            var result = JsonSerializer.Deserialize<JsonValue>(src);
            Assert.Equal(new JsonValue.Number(123), result);
        }

        [Fact]
        public void DeserializeBool()
        {
            var src = "true";
            var result = JsonSerializer.Deserialize<JsonValue>(src);
            Assert.Equal(new JsonValue.Bool(true), result);

            src = "false";
            result = JsonSerializer.Deserialize<JsonValue>(src);
            Assert.Equal(new JsonValue.Bool(false), result);
        }

        [GenerateDeserialize]
        private partial struct ExtraMembers
        {
            public int b;
        }

        [Fact]
        public void DeserializeExtraMembers()
        {
            var src = @"
{
    ""a"" : 1,
    ""c"" : {
        ""n1"": 1,
        ""n2"": 2
    },
    ""b"" : 2,
    ""d"" : [
        { ""n1"": 1 },
        2
    ]
}";
            var result = JsonSerializer.Deserialize<ExtraMembers>(src);
            Assert.Equal(2, result.b);
        }

        [GenerateDeserialize]
        [SerdeTypeOptions(MemberFormat = MemberFormat.CamelCase)]
        private partial struct IdStruct
        {
            public int Id;
        }

        [Fact]
        public void DeserializeId()
        {
            var src = @"{
                ""_links"":{
                    ""self"":{
                        ""href"":""https://dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_apis/pipelines/686?revision=12""
                    },
                    ""web"":{
                        ""href"":""https://dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_build/definition?definitionId=686""
                    }
                },
                ""configuration"":{
                    ""path"":""eng/pipelines/runtime.yml"",
                    ""repository"":{
                        ""fullName"":""dotnet/runtime"",
                        ""connection"":{
                            ""id"":""28d5e64a-b32c-4fc1-8d1b-8d741d67ee12""
                        },
                        ""type"":""gitHub""
                    },
                    ""type"":""yaml""
                },
                ""url"":""https://dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_apis/pipelines/686?revision=12"",
                ""id"":686,
                ""revision"":12,
                ""name"":""runtime"",
                ""folder"":""\\\\dotnet\\\\runtime""
            }";
            var result = JsonSerializer.Deserialize<IdStruct>(src);
            Assert.Equal(686, result.Id);
        }

        [GenerateDeserialize]
        [SerdeTypeOptions(MemberFormat = MemberFormat.CamelCase)]
        private readonly partial record struct IdStructList
        {
            public int Count { get; init; }
            public List<IdStruct> List { get; init; }
        }

        [Fact]
        public void DeserializeNestedWithList()
        {
            var listSrc = @"
[
    {
        ""skip1"": ""x"",
        ""id"": 1531298,
        ""skip2"": false,
        ""skip3"": null
    },
    {
        ""skip1"": ""y"",
        ""id"": 32414,
        ""skip2"": false,
        ""skip3"": null
    },
    {
        ""skip1"": ""z"",
        ""id"": 14254,
        ""skip2"": false,
        ""skip3"": null
    }
]";
            var src = @$"{{
  ""count"": 3,
  ""list"": {listSrc}
}}";

            var result = JsonSerializer.Deserialize<IdStructList>(src);
            Assert.Equal(result.Count, result.List.Count);
            Assert.Equal(1531298, result.List[0].Id);
            Assert.Equal(32414, result.List[1].Id);
            Assert.Equal(14254, result.List[2].Id);
            var listDirect = JsonSerializer.DeserializeList<IdStruct>(listSrc);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(result.List[i], listDirect[i]);
            }
        }

        [Fact]
        public void CheckSetToNull()
        {
            var src = @"
{
    ""present"": ""abc"",
    ""extra"": ""def""
}";
            var result = JsonSerializer.Deserialize<SetToNull>(src);
            Assert.Equal("abc", result.Present);
            Assert.Null(result.Missing);
        }

        [GenerateDeserialize]
        private readonly partial record struct SetToNull
        {
            public string Present { get; init; }
            public string? Missing { get; init; }
        }

        [GenerateDeserialize]
        private readonly partial record struct ThrowMissing
        {
            public string Present { get; init; }
            [SerdeMemberOptions(ThrowIfMissing = true)]
            public string? Missing { get; init; }
        }

        [Fact]
        public void ThrowIfMissing()
        {
            var src = @"
{
    ""present"": ""abc"",
    ""extra"": ""def""
}";
            Assert.Throws<InvalidDeserializeValueException>(() => JsonSerializer.Deserialize<ThrowMissing>(src));
        }


        [GenerateDeserialize]
        private partial class NullableFields
        {
            public string? S = null;
            public Dictionary<string, string?> Dict = new() {
                ["abc"] = null,
                ["def"] = "def"
            };
        }

        [Fact]
        public void NullableFieldsTest()
        {
            var src = @"
{
    ""dict"": {
        ""def"": ""def"",
        ""abc"": null
    }
}";
            var de = Serde.Json.JsonSerializer.Deserialize<NullableFields>(src);
            var s = new NullableFields();
            Assert.Equal(s.S, de.S);
            foreach (var (k, v) in s.Dict)
            {
                Assert.Equal(v, de.Dict[k]);
            }
            Assert.Equal(s.Dict.Count, de.Dict.Count);
        }


        partial class TestCase5
        {
            [GenerateSerialize, GenerateDeserialize]
            public partial record Type0
            {
                public Type1 Field0 { get; set; } = new Type1();
            }

            [GenerateSerialize, GenerateDeserialize]
            public partial record Type1
            {
                public List<int> Field0 { get; set; } = new List<int>()
            {int.MaxValue, int.MaxValue};
                public Type2 Field1 { get; set; } = new Type2();
                public Type3 Field2 { get; set; } = new Type3();
            }

            [GenerateSerialize, GenerateDeserialize]
            public partial record Type3
            {
                public bool Field0 { get; set; } = false;
            }

            [GenerateSerialize, GenerateDeserialize]
            public partial record Type2
            {
                public char Field0 { get; set; } = char.MaxValue;
            }
        }

        [Fact]
        public void CharMaxValue()
        {
            var src = """
{
    "field0": {
        "field0": [
            2147483647,
            2147483647
        ],
        "field1": {
            "field0": "\uFFFF"
        },
        "field2": {
            "field0": false
        }
    }
}
""";
            var de = JsonSerializer.Deserialize<TestCase5.Type0>(src);
            Assert.Equal(char.MaxValue, de.Field0.Field1.Field0);
        }

        public record Location : Serde.IDeserialize<Location>
        {
            public static readonly Location Sample = new Location
            {
                Id = 1234,
                Address1 = "The Street Name",
                Address2 = "20/11",
                City = "The City",
                State = "The State",
                PostalCode = "abc-12",
                Name = "Nonexisting",
                PhoneNumber = "+0 11 222 333 44",
                Country = "The Greatest"
            };

            public const string SerializedSample = """
{
    "id": 1234,
    "address1": "The Street Name",
    "address2": "20/11",
    "city": "The City",
    "state": "The State",
    "postalCode": "abc-12",
    "name": "Nonexisting",
    "phoneNumber": "+0 11 222 333 44",
    "country": "The Greatest"
}
""";
            public int Id { get; set; }
            public string Address1 { get; set; } = null!;
            public string Address2 { get; set; } = null!;
            public string City { get; set; } = null!;
            public string State { get; set; } = null!;
            public string PostalCode { get; set; } = null!;
            public string Name { get; set; } = null!;
            public string PhoneNumber { get; set; } = null!;
            public string Country { get; set; } = null!;

            public static Location Deserialize<D>(ref D deserializer)
                where D : IDeserializer
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[] { "Id", "Address1", "Address2", "City", "State", "PostalCode", "Name", "PhoneNumber", "Country" };
                return deserializer.DeserializeType<Location, SerdeVisitor>("Location", fieldNames, visitor);
            }

            private struct SerdeVisitor : IDeserializeVisitor<Location>
            {
                private struct FieldVisitor : IDeserializeVisitor<byte>, IDeserialize<byte>
                {
                    public string ExpectedTypeName => "UTF8 string";
                    static byte IDeserialize<byte>.Deserialize<D>(ref D deserializer)
                    {
                        return deserializer.DeserializeString<byte, FieldVisitor>(new FieldVisitor());
                    }

                    byte IDeserializeVisitor<byte>.VisitUtf8String(ReadOnlySpan<byte> utf8)
                    {
                        if (utf8.SequenceEqual("id"u8))
                        {
                            return 0;
                        }
                        else if (utf8.SequenceEqual("address1"u8))
                        {
                            return 1;
                        }
                        else if (utf8.SequenceEqual("address2"u8))
                        {
                            return 2;
                        }
                        else if (utf8.SequenceEqual("city"u8))
                        {
                            return 3;
                        }
                        else if (utf8.SequenceEqual("state"u8))
                        {
                            return 4;
                        }
                        else if (utf8.SequenceEqual("postalCode"u8))
                        {
                            return 5;
                        }
                        else if (utf8.SequenceEqual("name"u8))
                        {
                            return 6;
                        }
                        else if (utf8.SequenceEqual("phoneNumber"u8))
                        {
                            return 7;
                        }
                        else if (utf8.SequenceEqual("country"u8))
                        {
                            return 8;
                        }
                        else
                        {
                            return byte.MaxValue;
                        }
                    }
                }
                public string ExpectedTypeName => "Location";
                Location Serde.IDeserializeVisitor<Location>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<int> id = default;
                    Serde.Option<string> address1 = default;
                    Serde.Option<string> address2 = default;
                    Serde.Option<string> city = default;
                    Serde.Option<string> state = default;
                    Serde.Option<string> postalCode = default;
                    Serde.Option<string> name = default;
                    Serde.Option<string> phoneNumber = default;
                    Serde.Option<string> country = default;
                    while (d.TryGetNextKey<byte, FieldVisitor>(out byte key))
                    {
                        switch (key)
                        {
                            case 0:
                                id = d.GetNextValue<int, Int32Wrap>();
                                break;
                            case 1:
                                address1 = d.GetNextValue<string, StringWrap>();
                                break;
                            case 2:
                                address2 = d.GetNextValue<string, StringWrap>();
                                break;
                            case 3:
                                city = d.GetNextValue<string, StringWrap>();
                                break;
                            case 4:
                                state = d.GetNextValue<string, StringWrap>();
                                break;
                            case 5:
                                postalCode = d.GetNextValue<string, StringWrap>();
                                break;
                            case 6:
                                name = d.GetNextValue<string, StringWrap>();
                                break;
                            case 7:
                                phoneNumber = d.GetNextValue<string, StringWrap>();
                                break;
                            case 8:
                                country = d.GetNextValue<string, StringWrap>();
                                break;
                            default:
                                break;
                        }
                    }

                    var newType = new Location
                    { Id = id.GetValueOrThrow(), Address1 = address1.GetValueOrThrow(), Address2 = address2.GetValueOrThrow(), City = city.GetValueOrThrow(), State = state.GetValueOrThrow(), PostalCode = postalCode.GetValueOrThrow(), Name = name.GetValueOrThrow(), PhoneNumber = phoneNumber.GetValueOrThrow(), Country = country.GetValueOrThrow() };
                    return newType;
                }
            }
        }

        [Fact]
        public void Utf8StringTest()
        {
            var result = JsonSerializer.Deserialize<Location>(Location.SerializedSample);
            Assert.Equal(Location.Sample, result);
        }
    }
}