using Serde.Json;
using Xunit;
using IntArr = int[];
using StringArr = string[];
using PointArr = Serde.Test.RoundtripTests.Point[];

namespace Serde.Test;

public sealed partial class RoundtripTests
{
    [Fact]
    public void MaxSizeTypeTest()
    {
        var t = new MaxSizeType();
        AssertRoundTrip(t);
    }

    [Fact]
    public void TestArray()
    {
        AssertRoundTrip(
            new[] { 1, 2, 3 },
            IntArr.Serialize,
            IntArr.Deserialize);
        AssertRoundTrip(
            new[] { "a", "b", "c" },
            StringArr.Serialize,
            StringArr.Deserialize);
        AssertRoundTrip(
            new[] { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } },
            PointArr.Serialize,
            PointArr.Deserialize);
    }


    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record Point
    {
        public int X { get; init; }
        public int Y { get; init; }
    }

    private static void AssertRoundTrip<T>(T t) where T : ISerializeProvider<T>, IDeserializeProvider<T>
    {
        var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(t));
        Assert.Equal(t, result);
    }

    private static void AssertRoundTrip<T>(T expected, ISerialize<T> serializeImpl, IDeserialize<T> deserializeImpl)
    {
        var serialized = JsonSerializer.Serialize(expected, serializeImpl);
        var actual = JsonSerializer.Deserialize(serialized, deserializeImpl);
        Assert.Equal(expected, actual);
    }
}