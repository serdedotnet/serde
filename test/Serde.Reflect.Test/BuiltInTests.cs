using System;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using Serde.Json;
using Serde.Test;
using Xunit;

namespace Serde.Reflect.Test;

public class BuiltInTests
{
    private record StringTest : IReflectionSerialize<StringTest>, IReflectionDeserialize<StringTest>
    {
        public string F1 = "a";
        public string F2 = "b";
    }

    [Fact]
    public void String()
    {
        var json = """
{
  "F1": "a",
  "F2": "b"
}
""";
        VerifySerialize(json, new StringTest());
        VerifyDeserialize(json, new StringTest());
    }

    [Fact]
    public void AllInOneTest()
    {
        VerifySerialize(AllInOne.SampleJson, AllInOne.Sample);
        VerifyDeserialize(AllInOne.SampleJson, AllInOne.Sample);
    }

    private static void VerifySerialize<T>(string expected, T t) where T : ISerialize
    {
        var actual = PrettyPrint(JsonSerializer.Serialize(t));
        Assert.Equal(expected, actual);
    }

    private static void VerifyDeserialize<T>(string json, T expected)
        where T : IDeserialize<T>, IEquatable<T>
    {
        var actual = JsonSerializer.Deserialize<T>(json);
        Assert.Equal(expected, actual);
    }
    internal static string PrettyPrint(string json)
    {
        var doc = System.Text.Json.JsonDocument.Parse(json);
        var stream = new MemoryStream();
        var writer = new System.Text.Json.Utf8JsonWriter(stream, new System.Text.Json.JsonWriterOptions { Indented = true });
        doc.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

}