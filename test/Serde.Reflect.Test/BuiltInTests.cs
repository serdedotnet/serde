using Serde.Json;
using Serde.Test;
using Xunit;

namespace Serde.Reflect.Test;

public class BuiltInTests
{
    private class StringTest : IReflectionSerialize<StringTest>
    {
        public string F1 = "a";
        public string F2 = "b";
    }

    [Fact]
    public void String()
    {
        VerifyJson("""{"F1":"a","F2":"b"}""", new StringTest());
    }

    [Fact]
    public void AllInOneTest()
    {
        VerifyJson(AllInOne.SampleJson, AllInOne.Sample);
    }

    private static void VerifyJson<T>(string expected, T t) where T : ISerialize
    {
        var actual = JsonSerializer.Serialize(t);
        Assert.Equal(expected, actual);
    }
}