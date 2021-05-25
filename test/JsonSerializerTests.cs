using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Serde.Test
{
    public class JsonSerializerTests
    {
        private string PrettyPrint(string json)
        {
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            doc.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        abstract record JsonNode : ISerialize
        {
            internal JsonNode() { }

            public abstract void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
                where TSerializer : ISerializer<TSerializeType, TSerializeEnumerable>
                where TSerializeType : ISerializeType
                where TSerializeEnumerable : ISerializeEnumerable;

            public static implicit operator JsonNode(int i) => new JsonNumber(i);
        }
        abstract record JsonValue : JsonNode;
        partial record JsonNumber(int Value) : JsonValue;
        partial record JsonObject(IReadOnlyList<(string FieldName, JsonNode Node)> Members) : JsonNode;

        partial record JsonNumber
        {
            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            {
                serializer.Serialize(Value);
            }
        }

        partial record JsonObject
        {
            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
            {
                var type = serializer.SerializeType("", Members.Count);
                foreach (var (name, node) in Members)
                {
                    type.SerializeField(name, node);
                }
                type.End();
            }
        }

        private void VerifyJsonSource(JsonObject o, string expected)
        {
            var actual = JsonSerializer.WriteToString(o);
            Assert.Equal(expected.Trim(), PrettyPrint(actual));
        }

        [Fact]
        public void TestNestedTypes()
        {
            var src = new JsonObject(new (string, JsonNode)[] {
                ("field1", 1),
                ("field2", new JsonObject(new (string, JsonNode)[] {
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
    }
}