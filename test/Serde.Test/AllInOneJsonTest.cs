using Serde.Json;
using Xunit;
using STJ = System.Text.Json;

namespace Serde.Test;

public class AllInOneJsonTest
{
    [Fact]
    public void SerializeTest()
    {
        var actual = JsonSerializerTests.PrettyPrint(JsonSerializer.Serialize(AllInOne.Sample));
        Assert.Equal(AllInOne.SampleSerialized.Trim(), actual);
    }

    [Fact]
    public void DeserializeTest()
    {
        var actual = JsonSerializer.Deserialize<AllInOne>(AllInOne.SampleSerialized);
        Assert.Equal(AllInOne.Sample, actual);
    }

    [Fact]
    public void StjDeserializeTest()
    {
        var options = new STJ.JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNamingPolicy = STJ.JsonNamingPolicy.CamelCase,
            Converters =
            {
                new STJ.Serialization.JsonStringEnumConverter(STJ.JsonNamingPolicy.CamelCase)
            }
        };
        var actual = STJ.JsonSerializer.Deserialize<AllInOne>(AllInOne.SampleSerialized, options);
        Assert.Equal(AllInOne.Sample, actual);
    }
}
