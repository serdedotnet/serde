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
        internal static string PrettyPrint(string json)
        {
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var stream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            doc.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private void VerifyJsonSource(JsonNode node, string expected)
        {
            var actual = JsonSerializer.Serialize(node);
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

        [Fact]
        public void TestEnumerable()
        {
            var src = new JsonArray(ImmutableArray.Create<JsonNode>(
                1,
                2
            ));

            VerifyJsonSource(src, @"
[
  1,
  2
]");
        }

        [Fact]
        public void NestedEnumerable()
        {
            var src = new JsonArray(ImmutableArray.Create<JsonNode>(
                1,
                new JsonArray(ImmutableArray.Create<JsonNode>(
                    3,
                    4
                )),
                5,
                8
            ));

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
    }
}