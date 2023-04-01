using Serde.Json;
using Xunit;

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
}
