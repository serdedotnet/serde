
using Serde.Json;
using Xunit;
using static Serde.Json.JsonValue;

namespace Serde.Test
{
    public class JsonDeserializeTests
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
    }
}