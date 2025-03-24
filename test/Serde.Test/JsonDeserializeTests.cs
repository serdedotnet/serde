
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Reflection;
using System.Text;
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
            var result = JsonSerializer.DeserializeJsonValue(src);
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
            var result = JsonSerializer.DeserializeJsonValue(src);
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
            var result = JsonSerializer.DeserializeJsonValue(src);
            Assert.Equal(new JsonValue.Object(new (string, JsonValue)[] {
                ("field1", "abc"),
                ("field2", 123)
            }), result);
        }

        [Fact]
        public void DeserializeInt()
        {
            var src = "123";
            var result = JsonSerializer.DeserializeJsonValue(src);
            Assert.Equal(new JsonValue.Number(123), result);
        }

        [Fact]
        public void BadValueAtEnd()
        {
            var src = "123 456";
            Assert.Throws<JsonException>(() => JsonSerializer.DeserializeJsonValue(src));
        }

        [Fact]
        public void SingleElementObj()
        {
            var src = """
            { "field1": 123 }
            """;
            var result = JsonSerializer.DeserializeJsonValue(src);
            Assert.Equal(new JsonValue.Object([
                ("field1", 123)
            ]), result);
        }

        [Fact]
        public void DeserializeBool()
        {
            var src = "true";
            var result = JsonSerializer.DeserializeJsonValue(src);
            Assert.Equal(new JsonValue.Bool(true), result);

            src = "false";
            result = JsonSerializer.DeserializeJsonValue(src);
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
            Assert.Throws<DeserializeException>(() => JsonSerializer.Deserialize<ThrowMissing>(src));
        }

        [GenerateDeserialize]
        private partial record ThrowMissingFalse
        {
            public required string Present { get; init; }
            [SerdeMemberOptions(ThrowIfMissing = false)]
            public bool Missing { get; init; } = false;
        }

        [Fact]
        public void ThrowIfMissingFalse()
        {
            var src = """
{
    "present": "abc",
    "extra": "def"
}
""";
            var expected = new ThrowMissingFalse { Present = "abc", Missing = false };
            Assert.Equal(expected, JsonSerializer.Deserialize<ThrowMissingFalse>(src));
        }

        [Fact]
        public void DenyUnknownTest()
        {
            var src = @"
{
    ""present"": ""abc"",
    ""extra"": ""def""
}";
            Assert.Throws<DeserializeException>(() => JsonSerializer.Deserialize<DenyUnknown>(src));
        }

        [GenerateDeserialize]
        [SerdeTypeOptions(DenyUnknownMembers = true)]
        private readonly partial record struct DenyUnknown
        {
            public string Present { get; init; }
            public string? Missing { get; init; }
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

        [Fact]
        public void DeserializeWithSkip()
        {
            var src = """
{
    "required": "abc",
    "skip": "def"
}
""";
            var de = JsonSerializer.Deserialize<SkipDeserialize>(src);
            Assert.Equal(new SkipDeserialize { Required = "abc" }, de);
        }

        [GenerateDeserialize]
        public partial record SkipDeserialize
        {
            public required string Required { get; init; }

            [SerdeMemberOptions(SkipDeserialize = true)]
            public string Skip => "xyz";
        }

        [Fact]
        public void DeserializeEnum()
        {
            Assert.Equal(ColorEnum.Red, JsonSerializer.Deserialize<ColorEnum, ColorEnumWrap>("\"red\""));
        }

        private sealed class ColorEnumWrap : IDeserialize<ColorEnum>, IDeserializeProvider<ColorEnum>
        {
            public static ColorEnumWrap Instance { get; } = new();
            static IDeserialize<ColorEnum> IDeserializeProvider<ColorEnum>.Instance => Instance;
            private ColorEnumWrap() { }

            public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
                nameof(JsonDeserializeTests.ColorEnum),
                typeof(ColorEnum).GetCustomAttributesData(),
                I32Proxy.SerdeInfo,
                [
                    ("red", typeof(ColorEnum).GetField("Red")),
                    ("green", typeof(ColorEnum).GetField("Green")),
                    ("blue", typeof(ColorEnum).GetField("Blue")),
                ]
            );

            ColorEnum IDeserialize<ColorEnum>.Deserialize(IDeserializer deserializer)
            {
                var typeInfo = SerdeInfo;
                var de = deserializer.ReadType(typeInfo);
                int index;
                if ((index = de.TryReadIndex(typeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
                {
                    throw DeserializeException.UnknownMember(errorName!, typeInfo);
                }
                return index switch {
                    0 => ColorEnum.Red,
                    1 => ColorEnum.Green,
                    2 => ColorEnum.Blue,
                    _ => throw new System.InvalidOperationException($"Unexpected index: {index}")
                };
            }
        }

        private enum ColorEnum
        {
            Red,
            Green,
            Blue
        }

        [Fact]
        public void StringsWithEscapes()
        {
            var loc = JsonSerializer.Deserialize<Location>(Location.SampleString);
            Assert.Equal(Location.Sample, loc);
        }

        [GenerateDeserialize]
        public partial record Location
        {
            public int Id { get; set; }
            public required string Address1 { get; set; }
            public required string Address2 { get; set; }
            public required string City { get; set; }
            public required string State { get; set; }
            public required string PostalCode { get; set; }
            public required string Name { get; set; }
            public required string PhoneNumber { get; set; }
            public required string Country { get; set; }

            public const string SampleString = """
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

            public static Location Sample => new Location
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
        }

        [GenerateDeserialize]
        abstract partial record BasicDU
        {
            private BasicDU() { }

            public record A(int X) : BasicDU { }
            public record B(string Y) : BasicDU { }
        }

        [Fact]
        public void DeserializeBasicDU()
        {
            var aJson = """
            {"A":{"x":5}}
            """;
            var bJson = """
            {"B":{"y":"hello"}}
            """;
            var a = new BasicDU.A(5);
            var b = new BasicDU.B("hello");
            Assert.Equal(a, Serde.Json.JsonSerializer.Deserialize<BasicDU>(aJson));
            Assert.Equal(b, Serde.Json.JsonSerializer.Deserialize<BasicDU>(bJson));
        }

        private abstract partial record BasicDUManualTag : IDeserializeProvider<BasicDUManualTag>
        {
            private BasicDUManualTag() { }

            public record A(int W, int X) : BasicDUManualTag { }
            public record B(string Y, string Z) : BasicDUManualTag { }

            static IDeserialize<BasicDUManualTag> IDeserializeProvider<BasicDUManualTag>.Instance => _DeserializeObject.Instance;

            private sealed class _DeserializeObject : IDeserialize<BasicDUManualTag>
            {
                public static readonly _DeserializeObject Instance = new();

                public ISerdeInfo SerdeInfo => BasicDUManualTag.SerdeInfo;

                public BasicDUManualTag Deserialize(IDeserializer deserializer)
                {
                    var _l_baseInfo = BasicDUManualTag.SerdeInfo;
                    var typeDeserialize = deserializer.ReadType(_l_baseInfo);
                    if (typeDeserialize.TryReadIndex(_l_baseInfo, out var errorName) != 0)
                    {
                        throw new DeserializeException($"Unexpected key '{errorName}', expected union tag named 'tag'.");
                    }
                    var caseName = typeDeserialize.ReadString(_l_baseInfo, 0);
                    return caseName switch
                    {
                        nameof(A) => _m_AProxy.Instance.Deserialize(typeDeserialize),
                        nameof(B) => _m_BProxy.Instance.Deserialize(typeDeserialize),
                        _ => throw new DeserializeException($"Unknown union tag '{caseName}'."),
                    };
                }
            }

            private sealed class _m_AProxy : ISerdeInfoProvider
            {
                public static readonly _m_AProxy Instance = new();

                public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                    "A",
                    System.Array.Empty<CustomAttributeData>(),
                    [
                        ("w", I32Proxy.SerdeInfo, typeof(A).GetProperty("W")),
                        ("x", I32Proxy.SerdeInfo, typeof(A).GetProperty("X")),
                    ]);

                public A Deserialize(ITypeDeserializer typeDeserialize)
                {
                    var _l_AProxy = this.SerdeInfo;
                    int _l_index;
                    int _l_w = default;
                    int _l_x = default;
                    byte _r_assignedValid = 0;
                    while ((_l_index = typeDeserialize.TryReadIndex(_l_AProxy, out _)) != ITypeDeserializer.EndOfType)
                    {
                        switch (_l_index)
                        {
                            case 0:
                                _l_w = typeDeserialize.ReadI32(_l_AProxy, _l_index);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 1:
                                _l_x = typeDeserialize.ReadI32(_l_AProxy, _l_index);
                                _r_assignedValid |= ((byte)1) << 1;
                                break;
                            case Serde.ITypeDeserializer.IndexNotFound:
                                typeDeserialize.SkipValue(_l_AProxy, _l_index);
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index);
                        }
                    }

                    if ((_r_assignedValid & 0b11) != 0b11)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }

                    return new A(_l_w, _l_x);
                }
            }

            private sealed class _m_BProxy : ISerdeInfoProvider
            {
                public static readonly _m_BProxy Instance = new();
                public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                    "B",
                    System.Array.Empty<CustomAttributeData>(),
                    [
                        ("y", StringProxy.SerdeInfo, typeof(B).GetProperty("Y")),
                        ("z", StringProxy.SerdeInfo, typeof(B).GetProperty("Z")),
                    ]);
                public B Deserialize(ITypeDeserializer d)
                {
                    var _l_BProxy = this.SerdeInfo;
                    int _l_index;
                    string _l_y = default!;
                    string _l_z = default!;
                    byte _r_assignedValid = 0;
                    while ((_l_index = d.TryReadIndex(_l_BProxy, out _)) != ITypeDeserializer.EndOfType)
                    {
                        switch (_l_index)
                        {
                            case 0:
                                _l_y = d.ReadString(_l_BProxy, _l_index);
                                _r_assignedValid |= ((byte)1) << 0;
                                break;
                            case 1:
                                _l_z = d.ReadString(_l_BProxy, _l_index);
                                _r_assignedValid |= ((byte)1) << 1;
                                break;
                            case Serde.ITypeDeserializer.IndexNotFound:
                                d.SkipValue(_l_BProxy, _l_index);
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index);
                        }
                    }

                    if ((_r_assignedValid & 0b11) != 0b11)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }

                    return new B(_l_y, _l_z);
                }
            }

            private static ISerdeInfo SerdeInfo { get; } = new BaseSerdeInfo(
                nameof(BasicDUManualTag),
                "tag",
                [
                    _m_AProxy.Instance.SerdeInfo,
                    _m_BProxy.Instance.SerdeInfo,
                ]
            );

            private sealed record BaseSerdeInfo(
                string TypeName,
                string TagName,
                ImmutableArray<ISerdeInfo> CaseInfos
            ) : IUnionSerdeInfo
            {
                private readonly byte[] _utf8TagName = Encoding.UTF8.GetBytes(TagName);

                public string Name => TypeName;
                public IList<CustomAttributeData> Attributes => throw new System.NotImplementedException();
                public int FieldCount => 1;
                public IList<CustomAttributeData> GetFieldAttributes(int index) => index switch
                {
                    0 => [],
                    _ => throw new System.ArgumentOutOfRangeException(nameof(index)),
                };
                public ISerdeInfo GetFieldInfo(int index) => index switch
                {
                    0 => StringProxy.SerdeInfo,
                    _ => throw new System.ArgumentOutOfRangeException(nameof(index)),
                };
                public ReadOnlySpan<byte> GetFieldName(int index) => Encoding.UTF8.GetBytes(GetFieldStringName(index));
                public string GetFieldStringName(int index) => index switch
                {
                    0 => TagName,
                    _ => throw new System.ArgumentOutOfRangeException(nameof(index)),
                };
                public int TryGetIndex(ReadOnlySpan<byte> fieldName) => fieldName.SequenceEqual(_utf8TagName)
                    ? 0
                    : ITypeDeserializer.IndexNotFound;
            }
        }

        [Fact]
        public void SerializeBasicDUManualTag()
        {
            var aJson = """
            {"tag":"A","w":5,"x":6}
            """;
            var bJson = """
            {"tag":"B","y":"hello","z":"world"}
            """;
            Assert.Equal(new BasicDUManualTag.A(5, 6), Serde.Json.JsonSerializer.Deserialize<BasicDUManualTag>(aJson));
            Assert.Equal(new BasicDUManualTag.B("hello", "world"), Serde.Json.JsonSerializer.Deserialize<BasicDUManualTag>(bJson));
        }
    }
}