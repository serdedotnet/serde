
using Serde.Json;
using Xunit;

namespace Serde.Test;

public sealed class RoundtripTests
{
    [Fact]
    public void MaxSizeTypeTest()
    {
        var t = new MaxSizeType();
        AssertRoundTrip(t);
    }

    private static void AssertRoundTrip<T>(T t) where T : ISerialize<T>, IDeserialize<T>
    {
        var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(t));
        Assert.Equal(t, result);
    }
}