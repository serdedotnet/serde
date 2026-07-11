using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Serde.Json;
using Xunit;
using static Serde.Json.JsonValue;

namespace Serde.Test
{
    public partial class JsonSerializerTests
    {
        internal static string PrettyPrint(string json)
        {
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(
                stream,
                new JsonWriterOptions { Indented = true }
            );
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
            Assert.Equal(
                """
                {"value":"2023-10-01T12:00:00Z"}
                """,
                js
            );
        }

        [GenerateSerialize]
        private partial record DtWrap(System.DateTime Value);

        [Fact]
        public void DateTimeOffset()
        {
            var date = new DtoWrap(new(2023, 10, 1, 12, 0, 0, System.TimeSpan.FromHours(7)));
            var js = Serde.Json.JsonSerializer.Serialize(date);
            Assert.Equal(
                """
                {"value":"2023-10-01T12:00:00+07:00"}
                """,
                js
            );
            Assert.Equal(
                """
                {"value":"2023-10-01T12:00:00+07:00"}
                """,
                System.Text.Json.JsonSerializer.Serialize(
                    date,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    }
                )
            );
        }

        [GenerateSerialize]
        private partial record DtoWrap(System.DateTimeOffset Value);

        [Fact]
        public void SerializeRgb()
        {
            var color = new Color
            {
                Red = 3,
                Green = 5,
                Blue = 7,
            };
            Assert.Equal(
                """
                {"red":3,"green":5,"blue":7}
                """,
                Json.JsonSerializer.Serialize(color)
            );
        }

        [Fact]
        public void TestNestedTypes()
        {
            var src = new Object(
                new (string, JsonValue)[]
                {
                    ("field1", 1),
                    ("field2", new Object(new (string, JsonValue)[] { ("nested1", 5) })),
                    ("field3", 2),
                }
            );

            VerifyJsonSource(
                src,
                @"
{
  ""field1"": 1,
  ""field2"": {
    ""nested1"": 5
  },
  ""field3"": 2
}"
            );
        }

        [Fact]
        public void SerializeEnumerable()
        {
            var src = new Array([1, 2]);

            VerifyJsonSource(
                src,
                @"
[
  1,
  2
]"
            );
        }

        [Fact]
        public void NestedEnumerable()
        {
            var src = new Array([1, new Array([3, 4]), 5, 8]);

            VerifyJsonSource(
                src,
                @"
[
  1,
  [
    3,
    4
  ],
  5,
  8
]"
            );
        }

        private partial class JsonDictionaryWrapper(Dictionary<int, int> d)
            : IReadOnlyDictionary<int, int>
        {
            public Dictionary<int, int> _d = d;
            public int Count => _d.Count;
            public IEnumerable<int> Keys => _d.Keys;
            public IEnumerable<int> Values => _d.Values;

            public bool ContainsKey(int key) => _d.ContainsKey(key);

            public bool TryGetValue(int key, out int value) => _d.TryGetValue(key, out value);

            public IEnumerator<KeyValuePair<int, int>> GetEnumerator() => _d.GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
                GetEnumerator();

            public int this[int key] => _d[key];
        }

        partial class JsonDictionaryWrapper : ISerializeProvider<JsonDictionaryWrapper>
        {
            private sealed class ToStringProxy : ISerializeProvider<int>, ISerialize<int>
            {
                public static ISerialize<int> Instance { get; } = new ToStringProxy();

                public ISerdeInfo SerdeInfo => StringProxy.SerdeInfo;

                public void Serialize(int value, ISerializer serializer) =>
                    serializer.WriteString(value.ToString());

                void ISerialize<int>.SerializeAsField(
                    ITypeSerializer typeSerializer,
                    ISerdeInfo serdeInfo,
                    int index,
                    int value
                )
                {
                    typeSerializer.WriteString(serdeInfo, index, value.ToString());
                }
            }

            static ISerialize<JsonDictionaryWrapper> ISerializeProvider<JsonDictionaryWrapper>.Instance { get; } =
                new Proxy();

            private sealed class Proxy()
                : SerDictBase<Proxy, int, int, JsonDictionaryWrapper, ToStringProxy, I32Proxy>
            {
                private static readonly ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeDictionary(
                    typeof(Dictionary<int, int>).ToString(),
                    ToStringProxy.Instance.SerdeInfo,
                    I32Proxy.SerdeInfo
                );
                public override ISerdeInfo SerdeInfo => s_serdeInfo;
            }
        }

        [Fact]
        public void TestCustomDictionary()
        {
            var d = new Dictionary<int, int>() { [3] = 5, [1] = 10 };
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
            var js = Serde.Json.JsonSerializer.Serialize<
                string?,
                NullableRefProxy.Ser<string, StringProxy>
            >(s);
            Assert.Equal("null", js);
            js = Serde.Json.JsonSerializer.Serialize<JsonValue>(JsonValue.Null.Instance);
            Assert.Equal("null", js);
        }

        [GenerateSerde]
        private partial record NullableByteArrayWrap
        {
            public byte[]? Bytes { get; init; }
        }

        [Fact]
        public void NullableByteArray()
        {
            var wrap = new NullableByteArrayWrap { Bytes = new byte[] { 1, 2, 3 } };
            var js = Serde.Json.JsonSerializer.Serialize(wrap);
            Assert.Equal("""{"bytes":"AQID"}""", js);
            var de = Serde.Json.JsonSerializer.Deserialize<NullableByteArrayWrap>(js);
            Assert.Equal(wrap.Bytes, de.Bytes);

            var nullWrap = new NullableByteArrayWrap { Bytes = null };
            js = Serde.Json.JsonSerializer.Serialize(nullWrap);
            Assert.Equal("{}", js);
            de = Serde.Json.JsonSerializer.Deserialize<NullableByteArrayWrap>(js);
            Assert.Null(de.Bytes);
        }

        [GenerateSerialize]
        private partial class NullableFields
        {
            public string? S = null;
            public Dictionary<string, string?> D = new() { ["abc"] = null, ["def"] = "def" };
        }

        [GenerateSerialize]
        private partial struct Color
        {
            public int Red,
                Green,
                Blue;
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
            var arr = Serde.Json.JsonSerializer.Deserialize<int[], ArrayProxy.De<int, I32Proxy>>(
                js
            );
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
            Assert.Equal(
                """
                {"A":{"x":5}}
                """,
                Serde.Json.JsonSerializer.Serialize<BasicDU>(a)
            );
            Assert.Equal(
                """
                {"B":{"y":"hello"}}
                """,
                Serde.Json.JsonSerializer.Serialize<BasicDU>(b)
            );
        }

        abstract partial record BasicDUManualTag : ISerializeProvider<BasicDUManualTag>
        {
            private BasicDUManualTag() { }

            public record A(int W, int X) : BasicDUManualTag { }

            public record B(string Y, string Z) : BasicDUManualTag { }

            static ISerialize<BasicDUManualTag> ISerializeProvider<BasicDUManualTag>.Instance =>
                _SerializeObject.Instance;
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
                ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } =
                    Serde.SerdeInfo.MakeCustom(
                        "A",
                        System.Array.Empty<CustomAttributeData>(),
                        [
                            new Serde.SerdeInfo.FieldInfo("w", I32Proxy.SerdeInfo),
                            new Serde.SerdeInfo.FieldInfo("x", I32Proxy.SerdeInfo),
                        ]
                    );
            }

            private sealed class _m_BProxy : ISerdeInfoProvider
            {
                public static readonly _m_BProxy Instance = new();
                ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } =
                    Serde.SerdeInfo.MakeCustom(
                        "B",
                        System.Array.Empty<CustomAttributeData>(),
                        [
                            new Serde.SerdeInfo.FieldInfo("y", StringProxy.SerdeInfo),
                            new Serde.SerdeInfo.FieldInfo("z", StringProxy.SerdeInfo),
                        ]
                    );
            }

            private sealed class BaseSerdeInfo : IUnionSerdeInfo
            {
                public static readonly BaseSerdeInfo Instance = new BaseSerdeInfo();

                private BaseSerdeInfo() { }

                public ImmutableArray<ISerdeInfo> CaseInfos =>
                    throw new System.NotImplementedException();

                public string Name => throw new System.NotImplementedException();

                public IList<CustomAttributeData> Attributes =>
                    throw new System.NotImplementedException();

                public int FieldCount => throw new System.NotImplementedException();

                public IList<CustomAttributeData> GetFieldAttributes(int index) =>
                    throw new System.NotImplementedException();

                public ISerdeInfo GetFieldInfo(int index) =>
                    throw new System.NotImplementedException();

                public System.ReadOnlySpan<byte> GetFieldName(int index) =>
                    Encoding.UTF8.GetBytes(GetFieldStringName(index));

                public string GetFieldStringName(int index) =>
                    index switch
                    {
                        0 => "tag",
                        _ => throw new System.ArgumentOutOfRangeException(nameof(index)),
                    };

                public int TryGetIndex(System.ReadOnlySpan<byte> fieldName) =>
                    throw new System.NotImplementedException();
            }
        }

        [Fact]
        public void SerializeBasicDUManualTag()
        {
            var a = new BasicDUManualTag.A(5, 6);
            var b = new BasicDUManualTag.B("hello", "world");
            Assert.Equal(
                """
                {"tag":"A","w":5,"x":6}
                """,
                Serde.Json.JsonSerializer.Serialize<BasicDUManualTag>(a)
            );
            Assert.Equal(
                """
                {"tag":"B","y":"hello","z":"world"}
                """,
                Serde.Json.JsonSerializer.Serialize<BasicDUManualTag>(b)
            );
        }

        // A hand-written serializer that exercises the new WriteType(ISerdeInfo, int fieldCount)
        // overload together with the "IfNotNull" helpers that skip null fields. The field count
        // passed to WriteType reflects the number of fields that are actually written (i.e. the
        // total after nulls are skipped), which is what count-prefixed formats require.
        private sealed record ContactManual(int Id, string? Name, string? Email)
            : ISerializeProvider<ContactManual>
        {
            static ISerialize<ContactManual> ISerializeProvider<ContactManual>.Instance =>
                _Serialize.Instance;

            private static readonly ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                "ContactManual",
                System.Array.Empty<CustomAttributeData>(),
                [
                    new Serde.SerdeInfo.FieldInfo("id", I32Proxy.SerdeInfo),
                    new Serde.SerdeInfo.FieldInfo("name", StringProxy.SerdeInfo),
                    new Serde.SerdeInfo.FieldInfo("email", StringProxy.SerdeInfo),
                ]
            );

            private sealed class _Serialize : ISerialize<ContactManual>
            {
                public static readonly _Serialize Instance = new();

                public ISerdeInfo SerdeInfo => ContactManual.s_serdeInfo;

                public void Serialize(ContactManual value, ISerializer serializer)
                {
                    var info = this.SerdeInfo;
                    // Count only the fields that will actually be written so that formats which
                    // need the field count up front receive the post-skip total.
                    var fieldCount =
                        1 + (value.Name is null ? 0 : 1) + (value.Email is null ? 0 : 1);
                    var type = serializer.WriteType(info, fieldCount);
                    type.WriteI32(info, 0, value.Id);
                    type.WriteStringIfNotNull(info, 1, value.Name);
                    type.WriteStringIfNotNull(info, 2, value.Email);
                    type.End(info);
                }
            }
        }

        [Fact]
        public void WriteTypeWithFieldCountSkipsNullFields()
        {
            Assert.Equal(
                """{"id":1,"name":"Alice","email":"alice@example.com"}""",
                Serde.Json.JsonSerializer.Serialize(
                    new ContactManual(1, "Alice", "alice@example.com")
                )
            );

            // A null field is skipped by WriteStringIfNotNull; the remaining fields still serialize.
            Assert.Equal(
                """{"id":2,"email":"bob@example.com"}""",
                Serde.Json.JsonSerializer.Serialize(new ContactManual(2, null, "bob@example.com"))
            );

            Assert.Equal(
                """{"id":3,"name":"Carol"}""",
                Serde.Json.JsonSerializer.Serialize(new ContactManual(3, "Carol", null))
            );

            // When every optional field is null, only the required field remains.
            Assert.Equal(
                """{"id":4}""",
                Serde.Json.JsonSerializer.Serialize(new ContactManual(4, null, null))
            );
        }

        // Passes a deliberately inaccurate field count to WriteType to confirm that the JSON
        // serializer's default implementation ignores the count (JSON objects are structurally
        // delimited and do not need it) and produces the same output regardless.
        private sealed class WrongCountSerialize : ISerialize<ContactManual>
        {
            public ISerdeInfo SerdeInfo => SerdeInfoProvider.GetSerializeInfo<ContactManual>();

            public void Serialize(ContactManual value, ISerializer serializer)
            {
                var info = this.SerdeInfo;
                var type = serializer.WriteType(info, 0);
                type.WriteI32(info, 0, value.Id);
                type.WriteString(info, 1, value.Name!);
                type.WriteString(info, 2, value.Email!);
                type.End(info);
            }
        }

        [Fact]
        public void WriteTypeFieldCountIgnoredForJson()
        {
            var value = new ContactManual(7, "Carol", "carol@example.com");
            Assert.Equal(
                """{"id":7,"name":"Carol","email":"carol@example.com"}""",
                Serde.Json.JsonSerializer.Serialize(value, new WrongCountSerialize())
            );
        }

        [GenerateSerialize]
        private partial record BigData(List<int> Values);

        [Fact]
        public void ToBytesMatchesSerialize()
        {
            var color = new Color
            {
                Red = 3,
                Green = 5,
                Blue = 7,
            };
            var mem = Serde.Json.JsonSerializer.ToBytes(color);
            Assert.Equal(
                Serde.Json.JsonSerializer.Serialize(color),
                Encoding.UTF8.GetString(mem.Span)
            );
        }

        [Fact]
        public void ToBytesWithProvider()
        {
            var color = new Color
            {
                Red = 9,
                Green = 8,
                Blue = 7,
            };
            var mem = Serde.Json.JsonSerializer.ToBytes<Color>(color);
            Assert.Equal(
                Serde.Json.JsonSerializer.Serialize(color),
                Encoding.UTF8.GetString(mem.Span)
            );
        }

        [Fact]
        public void ToBytesGrowsBufferForLargeOutput()
        {
            var values = new List<int>();
            for (int i = 0; i < 5000; i++)
            {
                values.Add(i);
            }
            var data = new BigData(values);
            var mem = Serde.Json.JsonSerializer.ToBytes(data);
            Assert.Equal(
                Serde.Json.JsonSerializer.Serialize(data),
                Encoding.UTF8.GetString(mem.Span)
            );
        }

        [GenerateSerialize]
        private partial record ContactGen(int Id, string? Name, string? Email);

        [Theory]
        // Id is required; Name and Email are skipped when null, so the field count passed to
        // WriteType by the generated code must reflect only the fields actually written.
        [InlineData("Alice", "alice@example.com", 3)]
        [InlineData(null, "bob@example.com", 2)]
        [InlineData("Carol", null, 2)]
        [InlineData(null, null, 1)]
        public void GeneratedWriteTypeReceivesPostSkipFieldCount(
            string? name,
            string? email,
            int expectedFieldCount
        )
        {
            var recorder = new FieldCountRecordingSerializer();
            Serde
                .SerializeProvider.GetSerialize<ContactGen>()
                .Serialize(new ContactGen(1, name, email), recorder);
            Assert.Equal(expectedFieldCount, recorder.FieldCount);
        }

        /// <summary>
        /// A minimal <see cref="ISerializer"/>/<see cref="ITypeSerializer"/> that records the field
        /// count passed to <see cref="ISerializer.WriteType(ISerdeInfo, int)"/>. All other members
        /// are no-ops. Used to assert that generated code computes the correct post-skip count.
        /// </summary>
        private sealed class FieldCountRecordingSerializer : ISerializer, ITypeSerializer
        {
            public int? FieldCount { get; private set; }

            ITypeSerializer ISerializer.WriteType(ISerdeInfo info, int fieldCount)
            {
                FieldCount = fieldCount;
                return this;
            }

            ITypeSerializer ISerializer.WriteType(ISerdeInfo info) => this;

            ITypeSerializer ISerializer.WriteCollection(ISerdeInfo info, int? count) => this;

            // --- ISerializer no-op members ---
            void ISerializer.WriteBool(bool b) { }

            void ISerializer.WriteChar(char c) { }

            void ISerializer.WriteU8(byte b) { }

            void ISerializer.WriteU16(ushort u16) { }

            void ISerializer.WriteU32(uint u32) { }

            void ISerializer.WriteU64(ulong u64) { }

            void ISerializer.WriteU128(System.UInt128 u128) { }

            void ISerializer.WriteI8(sbyte b) { }

            void ISerializer.WriteI16(short i16) { }

            void ISerializer.WriteI32(int i32) { }

            void ISerializer.WriteI64(long i64) { }

            void ISerializer.WriteI128(System.Int128 i128) { }

            void ISerializer.WriteF32(float f) { }

            void ISerializer.WriteF64(double d) { }

            void ISerializer.WriteDecimal(decimal d) { }

            void ISerializer.WriteString(string s) { }

            void ISerializer.WriteNull() { }

            void ISerializer.WriteDateTime(System.DateTime dt) { }

            void ISerializer.WriteDateTimeOffset(System.DateTimeOffset dt) { }

            void ISerializer.WriteBytes(System.ReadOnlyMemory<byte> bytes) { }

            void ISerializer.WriteEnum(ISerdeInfo info, int ordinal) { }

            // --- ITypeSerializer members ---
            ISerializer ITypeSerializer.WriteFieldStart(ISerdeInfo typeInfo, int index) => this;

            void ITypeSerializer.WriteFieldEnd(
                ISerdeInfo typeInfo,
                int index,
                ISerializer serializer
            ) { }

            void ITypeSerializer.WriteBool(ISerdeInfo typeInfo, int index, bool b) { }

            void ITypeSerializer.WriteChar(ISerdeInfo typeInfo, int index, char c) { }

            void ITypeSerializer.WriteU8(ISerdeInfo typeInfo, int index, byte b) { }

            void ITypeSerializer.WriteU16(ISerdeInfo typeInfo, int index, ushort u16) { }

            void ITypeSerializer.WriteU32(ISerdeInfo typeInfo, int index, uint u32) { }

            void ITypeSerializer.WriteU64(ISerdeInfo typeInfo, int index, ulong u64) { }

            void ITypeSerializer.WriteU128(ISerdeInfo typeInfo, int index, System.UInt128 u128) { }

            void ITypeSerializer.WriteI8(ISerdeInfo typeInfo, int index, sbyte b) { }

            void ITypeSerializer.WriteI16(ISerdeInfo typeInfo, int index, short i16) { }

            void ITypeSerializer.WriteI32(ISerdeInfo typeInfo, int index, int i32) { }

            void ITypeSerializer.WriteI64(ISerdeInfo typeInfo, int index, long i64) { }

            void ITypeSerializer.WriteI128(ISerdeInfo typeInfo, int index, System.Int128 i128) { }

            void ITypeSerializer.WriteF32(ISerdeInfo typeInfo, int index, float f) { }

            void ITypeSerializer.WriteF64(ISerdeInfo typeInfo, int index, double d) { }

            void ITypeSerializer.WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) { }

            void ITypeSerializer.WriteString(ISerdeInfo typeInfo, int index, string s) { }

            void ITypeSerializer.WriteNull(ISerdeInfo typeInfo, int index) { }

            void ITypeSerializer.WriteDateTime(
                ISerdeInfo typeInfo,
                int index,
                System.DateTime dt
            ) { }

            void ITypeSerializer.WriteDateTimeOffset(
                ISerdeInfo typeInfo,
                int index,
                System.DateTimeOffset dt
            ) { }

            void ITypeSerializer.WriteBytes(
                ISerdeInfo typeInfo,
                int index,
                System.ReadOnlyMemory<byte> bytes
            ) { }

            void ITypeSerializer.WriteEnum(
                ISerdeInfo typeInfo,
                int index,
                ISerdeInfo fieldInfo,
                int ordinal
            ) { }

            void ITypeSerializer.WriteValue<T>(
                ISerdeInfo typeInfo,
                int index,
                T value,
                ISerialize<T> serialize
            ) { }

            void ITypeSerializer.End(ISerdeInfo info) { }
        }
    }
}
