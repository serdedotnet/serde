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

        private struct JsonDictionaryWrapper : ISerialize<JsonDictionaryWrapper>, ISerializeProvider<JsonDictionaryWrapper>
        {
            public static JsonDictionaryWrapper Instance { get; } = new();
            static ISerialize<JsonDictionaryWrapper> ISerializeProvider<JsonDictionaryWrapper>.SerializeInstance => Instance;

            public static ISerdeInfo SerdeInfo { get; } = new CollectionSerdeInfo(
                typeof(Dictionary<int, int>).ToString(),
                InfoKind.Dictionary);

            private readonly Dictionary<int, int> _d;
            public JsonDictionaryWrapper(Dictionary<int, int> d)
            {
                _d = d;
            }
            public void Serialize(JsonDictionaryWrapper value, ISerializer serializer)
            {
                var typeInfo = SerdeInfo;
                var sd = serializer.WriteCollection(typeInfo, value._d.Count);
                foreach (var (k,v) in value._d)
                {
                    sd.WriteElement(k.ToString(), StringProxy.Instance);
                    sd.WriteElement(v, Int32Proxy.Instance);
                }
                sd.End(typeInfo);
            }
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
            var js = Serde.Json.JsonSerializer.Serialize<string?, NullableRefProxy.Serialize<string, StringProxy>>(s);
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
            var arr = Serde.Json.JsonSerializer.Deserialize<int[], ArrayProxy.Deserialize<int, Int32Proxy>>(js);
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

        abstract partial record BasicDUManualTag : ISerdeInfoProvider, ISerializeProvider<BasicDUManualTag>
        {
            private BasicDUManualTag() { }

            public record A(int W, int X) : BasicDUManualTag { }
            public record B(string Y, string Z) : BasicDUManualTag { }

            static ISerialize<BasicDUManualTag> ISerializeProvider<BasicDUManualTag>.SerializeInstance => _SerializeObject.Instance;

            private sealed class _SerializeObject : ISerialize<BasicDUManualTag>
            {
                public static readonly _SerializeObject Instance = new();

                public void Serialize(BasicDUManualTag value, ISerializer serializer)
                {
                    var _l_baseInfo = SerdeInfoProvider.GetInfo<BasicDUManualTag>();
                    var type = serializer.WriteType(_l_baseInfo);
                    switch (value)
                    {
                        case BasicDUManualTag.A c:
                        {
                            type.WriteString(_l_baseInfo, 0, "A");
                            var caseInfo = SerdeInfoProvider.GetInfo<_m_AProxy>();
                            type.WriteI32(caseInfo, 0, c.W);
                            type.WriteI32(caseInfo, 1, c.X);
                            break;
                        }
                        case BasicDUManualTag.B c:
                        {
                            type.WriteString(_l_baseInfo, 0, "B");
                            var caseInfo = SerdeInfoProvider.GetInfo<_m_BProxy>();
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
                static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = SerdeInfo.MakeCustom(
                    "A",
                    System.Array.Empty<CustomAttributeData>(),
                    [
                        ("w", SerdeInfoProvider.GetInfo<Int32Proxy>(), typeof(A).GetProperty("W")!),
                        ("x", SerdeInfoProvider.GetInfo<Int32Proxy>(), typeof(A).GetProperty("X")!),
                    ]);
            }

            private sealed class _m_BProxy : ISerdeInfoProvider
            {
                static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = SerdeInfo.MakeCustom(
                    "B",
                    System.Array.Empty<CustomAttributeData>(),
                    [
                        ("y", SerdeInfoProvider.GetInfo<StringProxy>(), typeof(B).GetProperty("Y")!),
                        ("z", SerdeInfoProvider.GetInfo<StringProxy>(), typeof(B).GetProperty("Z")!),
                    ]);
            }

            static ISerdeInfo ISerdeInfoProvider.SerdeInfo => BaseSerdeInfo.Instance;

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
