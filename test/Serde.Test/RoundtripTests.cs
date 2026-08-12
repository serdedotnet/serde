using Serde.Json;
using Xunit;
using IntArr = int[];
using PointArr = Serde.Test.RoundtripTests.Point[];
using StringArr = string[];

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
        AssertRoundTrip(new[] { 1, 2, 3 }, IntArr.Serialize, IntArr.Deserialize);
        AssertRoundTrip(new[] { "a", "b", "c" }, StringArr.Serialize, StringArr.Deserialize);
        AssertRoundTrip(
            new[]
            {
                new Point { X = 1, Y = 2 },
                new Point { X = 3, Y = 4 },
            },
            PointArr.Serialize,
            PointArr.Deserialize
        );
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record Point
    {
        public int X { get; init; }
        public int Y { get; init; }
    }

    /// <summary>
    /// A "foreign" type with no parameterless constructor and read-only members,
    /// serialized through a non-empty proxy that converts to and from it.
    /// </summary>
    public sealed class ForeignPoint(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    [GenerateSerde(ForType = typeof(ForeignPoint))]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial struct ForeignPointProxy
    {
        public int X;
        public int Y;

        public static explicit operator ForeignPoint(ForeignPointProxy p) =>
            new ForeignPoint(p.X, p.Y);

        public static explicit operator ForeignPointProxy(ForeignPoint p) =>
            new ForeignPointProxy { X = p.X, Y = p.Y };
    }

    [Fact]
    public void NonEmptyConversionProxyRoundtrip()
    {
        var p = new ForeignPoint(3, 7);
        var json = JsonSerializer.Serialize<ForeignPoint, ForeignPointProxy>(p);
        Assert.Equal("""{"X":3,"Y":7}""", json);

        var back = JsonSerializer.Deserialize<ForeignPoint, ForeignPointProxy>(json);
        Assert.Equal(p.X, back.X);
        Assert.Equal(p.Y, back.Y);
    }

    [GenerateSerde]
    [UseProxy(ForType = typeof(ForeignPoint), Proxy = typeof(ForeignPointProxy))]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record ProxiedContainer
    {
        public required ForeignPoint Point { get; init; }
        public required ForeignPoint? OptionalPoint { get; init; }
    }

    /// <summary>
    /// A type-scoped proxy registered with <c>[UseProxy]</c> also applies to nullable reference
    /// members of that type, by composing with <c>NullableRefProxy</c>.
    /// </summary>
    [Fact]
    public void UseProxyWithNullableRefMemberRoundtrip()
    {
        var c = new ProxiedContainer
        {
            Point = new ForeignPoint(1, 2),
            OptionalPoint = new ForeignPoint(3, 4),
        };
        var json = JsonSerializer.Serialize(c);
        Assert.Equal("""{"Point":{"X":1,"Y":2},"OptionalPoint":{"X":3,"Y":4}}""", json);

        var back = JsonSerializer.Deserialize<ProxiedContainer>(json);
        Assert.Equal(3, back.OptionalPoint!.X);
        Assert.Equal(4, back.OptionalPoint.Y);

        var withNull = c with { OptionalPoint = null };
        var nullJson = JsonSerializer.Serialize(withNull);
        Assert.Equal("""{"Point":{"X":1,"Y":2}}""", nullJson);
        Assert.Null(JsonSerializer.Deserialize<ProxiedContainer>(nullJson).OptionalPoint);
    }

    private static void AssertRoundTrip<T>(T t)
        where T : ISerializeProvider<T>, IDeserializeProvider<T>
    {
        var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(t));
        Assert.Equal(t, result);
    }

    private static void AssertRoundTrip<T>(
        T expected,
        ISerialize<T> serializeImpl,
        IDeserialize<T> deserializeImpl
    )
    {
        var serialized = JsonSerializer.Serialize(expected, serializeImpl);
        var actual = JsonSerializer.Deserialize(serialized, deserializeImpl);
        Assert.Equal(expected, actual);
    }
}
