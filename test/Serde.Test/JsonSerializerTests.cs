using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serde.Json;
using Xunit;
using Xunit.Sdk;
using static Serde.Json.JsonValue;
using static Serde.Test.JsonSerializerTests;

namespace Serde.Test
{
    public partial class JsonSerializerTests
    {
        internal static string PrettyPrint(string json)
        {
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            doc.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private void VerifyJsonSource(JsonValue node, string expected)
        {
            var actual = Serde.Json.JsonSerializer.Serialize(node);
            using var doc = JsonDocument.Parse(actual);
            Assert.Equal(expected.Trim(), PrettyPrint(actual));
        }

        [Fact]
        public void DateTime()
        {
            var date = new DtWrap(new(2023, 10, 1, 12, 0, 0, System.DateTimeKind.Utc));
            var js = Serde.Json.JsonSerializer.Serialize(date);
            Assert.Equal("""
            {"value":"2023-10-01T12:00:00Z"}
            """, js);
        }

        [GenerateSerialize]
        private partial record DtWrap(System.DateTime Value);


        [Fact]
        public void DateTimeOffset()
        {
            var date = new DtoWrap(new(2023, 10, 1, 12, 0, 0, System.TimeSpan.FromHours(7)));
            var js = Serde.Json.JsonSerializer.Serialize(date);
            Assert.Equal("""
            {"value":"2023-10-01T12:00:00+07:00"}
            """, js);
            Assert.Equal("""
            {"value":"2023-10-01T12:00:00+07:00"}
            """, System.Text.Json.JsonSerializer.Serialize(date, new JsonSerializerOptions()
                { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }

        [GenerateSerialize]
        private partial record DtoWrap(System.DateTimeOffset Value);

        [Fact]
        public void SerializeRgb()
        {
            var color = new Color { Red = 3, Green = 5, Blue = 7 };
            Assert.Equal("""
            {"red":3,"green":5,"blue":7}
            """, Json.JsonSerializer.Serialize(color));
        }

        [Fact]
        public void TestNestedTypes()
        {
            var src = new Object(new (string, JsonValue)[] {
                ("field1", 1),
                ("field2", new Object(new (string, JsonValue)[] {
                    ("nested1", 5)
                })),
                ("field3", 2)
            });

            VerifyJsonSource(src, @"
{
  ""field1"": 1,
  ""field2"": {
    ""nested1"": 5
  },
  ""field3"": 2
}");
        }

        [Fact]
        public void SerializeEnumerable()
        {
            var src = new Array([1, 2
]);

            VerifyJsonSource(src, @"
[
  1,
  2
]");
        }

        [Fact]
        public void NestedEnumerable()
        {
            var src = new Array(
            [
                1,
                new Array([3, 4
]),
                5,
                8
,
            ]);

            VerifyJsonSource(src, @"
[
  1,
  [
    3,
    4
  ],
  5,
  8
]");
        }

        private partial class JsonDictionaryWrapper(Dictionary<int, int> d) : IReadOnlyDictionary<int, int>
        {
            public Dictionary<int, int> _d = d;
            public int Count => _d.Count;
            public IEnumerable<int> Keys => _d.Keys;
            public IEnumerable<int> Values => _d.Values;
            public bool ContainsKey(int key) => _d.ContainsKey(key);
            public bool TryGetValue(int key, out int value) => _d.TryGetValue(key, out value);
            public IEnumerator<KeyValuePair<int, int>> GetEnumerator() => _d.GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public int this[int key] => _d[key];
        }
        partial class JsonDictionaryWrapper : ISerializeProvider<JsonDictionaryWrapper>
        {
            private sealed class ToStringProxy : ISerializeProvider<int>, ISerialize<int>, ITypeSerialize<int>
            {
                public static ISerialize<int> Instance { get; } = new ToStringProxy();

                public ISerdeInfo SerdeInfo => StringProxy.SerdeInfo;

                public void Serialize(int value, ISerializer serializer) => serializer.WriteString(value.ToString());

                public void Serialize(int value, ITypeSerializer serializer, ISerdeInfo info, int index)
                {
                    serializer.WriteString(info, index, value.ToString());
                }
            }

            static ISerialize<JsonDictionaryWrapper> ISerializeProvider<JsonDictionaryWrapper>.Instance { get; }
                = new Proxy();

            private sealed class Proxy()
                : SerDictBase<Proxy, int, int, JsonDictionaryWrapper, ToStringProxy, I32Proxy>(
                    new CollectionSerdeInfo(
                        typeof(Dictionary<int, int>).ToString(),
                        InfoKind.Dictionary
                    )
                )
            { }
        }

        [Fact]
        public void TestCustomDictionary()
        {
            var d = new Dictionary<int, int>()
            {
                [3] = 5,
                [1] = 10
            };
            var js = Serde.Json.JsonSerializer.Serialize(new JsonDictionaryWrapper(d));
            var resultDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, int>>(js)!;
            Assert.Equal(d.Count, resultDict.Count);
            foreach (var (k, v) in resultDict)
            {
                Assert.Equal(d[k], v);
            }
        }

        [Fact]
        public void NullableString()
        {
            string? s = null;
            var js = Serde.Json.JsonSerializer.Serialize<string?, NullableRefProxy.Ser<string, StringProxy>>(s);
            Assert.Equal("null", js);
            js = Serde.Json.JsonSerializer.Serialize<JsonValue>(JsonValue.Null.Instance);
            Assert.Equal("null", js);
        }

        [GenerateSerialize]
        private partial class NullableFields
        {
            public string? S = null;
            public Dictionary<string, string?> D = new() {
                ["abc"] = null,
                ["def"] = "def"
            };
        }

        [GenerateSerialize]
        private partial struct Color
        {
            public int Red, Green, Blue;
        }

        [Fact]
        public void NullableFieldsTest()
        {
            var s = new NullableFields();
            var js = Serde.Json.JsonSerializer.Serialize(s);
            var de = System.Text.Json.JsonSerializer.Deserialize<NullableFields>(js);
            Debug.Assert(de != null);
            Assert.Equal(s.S, de.S);
            foreach (var (k, v) in s.D)
            {
                Assert.Equal(v, de.D[k]);
            }
            Assert.Equal(s.D.Count, de.D.Count);
        }

        [Fact]
        public void DeserializeIntArray()
        {
            var js = "[1,2,3]";
            var arr = Serde.Json.JsonSerializer.Deserialize<int[], ArrayProxy.De<int, I32Proxy>>(js);
            Assert.Equal(new int[] { 1, 2, 3 }, arr);
        }

        [GenerateSerialize]
        abstract partial record BasicDU
        {
            private BasicDU() { }

            public record A(int X) : BasicDU { }
            public record B(string Y) : BasicDU { }
        }

        [Fact]
        public void SerializeBasicDU()
        {
            var a = new BasicDU.A(5);
            var b = new BasicDU.B("hello");
            Assert.Equal("""
            {"A":{"x":5}}
            """, Serde.Json.JsonSerializer.Serialize<BasicDU>(a));
            Assert.Equal("""
            {"B":{"y":"hello"}}
            """, Serde.Json.JsonSerializer.Serialize<BasicDU>(b));
        }

        abstract partial record BasicDUManualTag : ISerializeProvider<BasicDUManualTag>
        {
            private BasicDUManualTag() { }

            public record A(int W, int X) : BasicDUManualTag { }
            public record B(string Y, string Z) : BasicDUManualTag { }

            static ISerialize<BasicDUManualTag> ISerializeProvider<BasicDUManualTag>.Instance => _SerializeObject.Instance;
            private static ISerdeInfo SerdeInfo => BaseSerdeInfo.Instance;


            private sealed class _SerializeObject : ISerialize<BasicDUManualTag>
            {
                public static readonly _SerializeObject Instance = new();

                public ISerdeInfo SerdeInfo => BasicDUManualTag.SerdeInfo;

                public void Serialize(BasicDUManualTag value, ISerializer serializer)
                {
                    var _l_baseInfo = this.SerdeInfo;
                    var type = serializer.WriteType(_l_baseInfo);
                    switch (value)
                    {
                        case BasicDUManualTag.A c:
                        {
                            type.WriteString(_l_baseInfo, 0, "A");
                            var caseInfo = SerdeInfoProvider.GetInfo(_m_AProxy.Instance);
                            type.WriteI32(caseInfo, 0, c.W);
                            type.WriteI32(caseInfo, 1, c.X);
                            break;
                        }
                        case BasicDUManualTag.B c:
                        {
                            type.WriteString(_l_baseInfo, 0, "B");
                            var caseInfo = SerdeInfoProvider.GetInfo(_m_BProxy.Instance);
                            type.WriteString(caseInfo, 0, c.Y);
                            type.WriteString(caseInfo, 1, c.Z);
                            break;
                        }
                    }
                    type.End(_l_baseInfo);
                }
            }

            private sealed class _m_AProxy : ISerdeInfoProvider
            {
                public static readonly _m_AProxy Instance = new();
                ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                    "A",
                    System.Array.Empty<CustomAttributeData>(),
                    [
                        ("w", I32Proxy.SerdeInfo, typeof(A).GetProperty("W")),
                        ("x", I32Proxy.SerdeInfo, typeof(A).GetProperty("X")),
                    ]);
            }

            private sealed class _m_BProxy : ISerdeInfoProvider
            {
                public static readonly _m_BProxy Instance = new();
                ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
                    "B",
                    System.Array.Empty<CustomAttributeData>(),
                    [
                        ("y", StringProxy.SerdeInfo, typeof(B).GetProperty("Y")),
                        ("z", StringProxy.SerdeInfo, typeof(B).GetProperty("Z")),
                    ]);
            }

            private sealed class BaseSerdeInfo : IUnionSerdeInfo
            {
                public static readonly BaseSerdeInfo Instance = new BaseSerdeInfo();
                private BaseSerdeInfo() { }

                public ImmutableArray<ISerdeInfo> CaseInfos => throw new System.NotImplementedException();

                public string Name => throw new System.NotImplementedException();

                public IList<CustomAttributeData> Attributes => throw new System.NotImplementedException();

                public int FieldCount => throw new System.NotImplementedException();

                public IList<CustomAttributeData> GetFieldAttributes(int index) => throw new System.NotImplementedException();

                public ISerdeInfo GetFieldInfo(int index) => throw new System.NotImplementedException();

                public System.ReadOnlySpan<byte> GetFieldName(int index) => Encoding.UTF8.GetBytes(GetFieldStringName(index));

                public string GetFieldStringName(int index) => index switch
                {
                    0 => "tag",
                    _ => throw new System.ArgumentOutOfRangeException(nameof(index)),
                };

                public int TryGetIndex(System.ReadOnlySpan<byte> fieldName) => throw new System.NotImplementedException();
            }
        }

        [Fact]
        public void SerializeBasicDUManualTag()
        {
            var a = new BasicDUManualTag.A(5, 6);
            var b = new BasicDUManualTag.B("hello", "world");
            Assert.Equal("""
            {"tag":"A","w":5,"x":6}
            """, Serde.Json.JsonSerializer.Serialize<BasicDUManualTag>(a));
            Assert.Equal("""
            {"tag":"B","y":"hello","z":"world"}
            """, Serde.Json.JsonSerializer.Serialize<BasicDUManualTag>(b));
        }
    }
}
