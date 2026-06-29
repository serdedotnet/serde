using System.Collections.Generic;
using Serde;
using Serde.Json;
using Xunit;
using Point = Serde.Test.RoundtripTests.Point;

namespace Serde.Test;

public sealed partial class TupleTests
{
    [Fact]
    public void OneTuple()
    {
        var v = new System.ValueTuple<int>(42);
        var json = JsonSerializer.Serialize<System.ValueTuple<int>>(
            v,
            TupleProxy.Ser<int, I32Proxy>.Instance
        );
        Assert.Equal("[42]", json);

        var back = JsonSerializer.Deserialize<System.ValueTuple<int>>(
            json,
            TupleProxy.De<int, I32Proxy>.Instance
        );
        Assert.Equal(v, back);
    }

    [Fact]
    public void TwoTuple()
    {
        var v = (1, "hi");
        var json = JsonSerializer.Serialize<(int, string)>(
            v,
            TupleProxy.Ser<int, string, I32Proxy, StringProxy>.Instance
        );
        Assert.Equal("""[1,"hi"]""", json);

        var back = JsonSerializer.Deserialize<(int, string)>(
            json,
            TupleProxy.De<int, string, I32Proxy, StringProxy>.Instance
        );
        Assert.Equal(v, back);
    }

    [Fact]
    public void SevenTuple()
    {
        var v = (1, 2, 3, 4, 5, 6, 7);
        var ser = TupleProxy
            .Ser<
                int,
                int,
                int,
                int,
                int,
                int,
                int,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy
            >
            .Instance;
        var json = JsonSerializer.Serialize<(int, int, int, int, int, int, int)>(v, ser);
        Assert.Equal("[1,2,3,4,5,6,7]", json);

        var de = TupleProxy
            .De<
                int,
                int,
                int,
                int,
                int,
                int,
                int,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy,
                I32Proxy
            >
            .Instance;
        var back = JsonSerializer.Deserialize<(int, int, int, int, int, int, int)>(json, de);
        Assert.Equal(v, back);
    }

    [Fact]
    public void TupleWithComplexElement()
    {
        var v = (5, new Point { X = 1, Y = 2 });
        var json = JsonSerializer.Serialize<(int, Point)>(
            v,
            TupleProxy.Ser<int, Point, I32Proxy, Point>.Instance
        );
        Assert.Equal("""[5,{"X":1,"Y":2}]""", json);

        var back = JsonSerializer.Deserialize<(int, Point)>(
            json,
            TupleProxy.De<int, Point, I32Proxy, Point>.Instance
        );
        Assert.Equal(v, back);
    }

    [Fact]
    public void NestedTuple()
    {
        var v = (1, (2, 3));
        var ser = TupleProxy
            .Ser<int, (int, int), I32Proxy, TupleProxy.Ser<int, int, I32Proxy, I32Proxy>>
            .Instance;
        var json = JsonSerializer.Serialize<(int, (int, int))>(v, ser);
        Assert.Equal("[1,[2,3]]", json);

        var de = TupleProxy
            .De<int, (int, int), I32Proxy, TupleProxy.De<int, int, I32Proxy, I32Proxy>>
            .Instance;
        var back = JsonSerializer.Deserialize<(int, (int, int))>(json, de);
        Assert.Equal(v, back);
    }

    [Fact]
    public void TupleInList()
    {
        var v = new List<(int, int)> { (1, 2), (3, 4) };
        var ser = ListProxy.Ser<(int, int), TupleProxy.Ser<int, int, I32Proxy, I32Proxy>>.Instance;
        var json = JsonSerializer.Serialize<List<(int, int)>>(v, ser);
        Assert.Equal("[[1,2],[3,4]]", json);

        var de = ListProxy.De<(int, int), TupleProxy.De<int, int, I32Proxy, I32Proxy>>.Instance;
        var back = JsonSerializer.Deserialize<List<(int, int)>>(json, de);
        Assert.Equal(v, back);
    }

    [Fact]
    public void TooFewElementsThrows()
    {
        var de = TupleProxy.De<int, int, I32Proxy, I32Proxy>.Instance;
        Assert.Throws<DeserializeException>(() =>
            JsonSerializer.Deserialize<(int, int)>("[1]", de)
        );
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record TupleHolder
    {
        public (int, string) Pair { get; init; }
        public (int, (string, bool)) Nested { get; init; }
        public List<(int, int)> Points { get; init; } = new();
    }

    [Fact]
    public void GeneratedTupleMembersRoundtrip()
    {
        var v = new TupleHolder
        {
            Pair = (1, "a"),
            Nested = (2, ("b", true)),
            Points = new List<(int, int)> { (3, 4), (5, 6) },
        };
        var json = JsonSerializer.Serialize(v);
        Assert.Equal("""{"Pair":[1,"a"],"Nested":[2,["b",true]],"Points":[[3,4],[5,6]]}""", json);

        var back = JsonSerializer.Deserialize<TupleHolder>(json);
        Assert.Equal(v.Pair, back.Pair);
        Assert.Equal(v.Nested, back.Nested);
        Assert.Equal(v.Points, back.Points);
    }
}
