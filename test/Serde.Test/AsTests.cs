using Serde.Json;
using Xunit;

namespace Serde.Test;

public partial class AsTests
{
    [GenerateSerde(As = typeof(string))]
    public readonly partial struct StringId
    {
        public readonly string Value;
        public StringId(string value) => Value = value;

        public static explicit operator string(StringId id) => id.Value;
        public static explicit operator StringId(string value) => new StringId(value);
    }

    [Fact]
    public void RoundTripStringId()
    {
        var id = new StringId("hello");
        var json = JsonSerializer.Serialize(id);
        Assert.Equal("\"hello\"", json);

        var back = JsonSerializer.Deserialize<StringId>(json);
        Assert.Equal("hello", back.Value);
    }

    [GenerateSerde]
    public partial record Point(int X, int Y);

    [GenerateSerde(As = typeof(Point))]
    public readonly partial struct PointWrapper
    {
        public readonly int X;
        public readonly int Y;
        public PointWrapper(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point(PointWrapper p) => new Point(p.X, p.Y);
        public static implicit operator PointWrapper(Point p) => new PointWrapper(p.X, p.Y);
    }

    [Fact]
    public void RoundTripPointWrapper()
    {
        var p = new PointWrapper(1, 2);
        var json = JsonSerializer.Serialize(p);
        Assert.Equal("""{"x":1,"y":2}""", json);

        var back = JsonSerializer.Deserialize<PointWrapper>(json);
        Assert.Equal(1, back.X);
        Assert.Equal(2, back.Y);
    }

    [GenerateSerialize(As = typeof(string))]
    public readonly partial record struct SerOnlyId(int Value)
    {
        public static explicit operator string(SerOnlyId id) => id.Value.ToString();
    }

    [Fact]
    public void SerializeOnlyAsString()
    {
        var id = new SerOnlyId(42);
        var json = JsonSerializer.Serialize<SerOnlyId, SerOnlyId>(id);
        Assert.Equal("\"42\"", json);
    }

    [GenerateDeserialize(As = typeof(string))]
    public readonly partial record struct DeOnlyId(int Value)
    {
        public static explicit operator DeOnlyId(string value) => new DeOnlyId(int.Parse(value));
    }

    [Fact]
    public void DeserializeOnlyAsString()
    {
        var id = JsonSerializer.Deserialize<DeOnlyId, DeOnlyId>("\"42\"");
        Assert.Equal(42, id.Value);
    }

    [GenerateSerde(As = typeof(System.ValueTuple<int, string>))]
    public readonly partial record struct TupleWrapper(int Item1, string Item2);

    [Fact]
    public void RoundTripTupleWrapper()
    {
        var w = new TupleWrapper(1, "hello");
        var json = JsonSerializer.Serialize(w);
        Assert.Equal("""[1,"hello"]""", json);

        var back = JsonSerializer.Deserialize<TupleWrapper>(json);
        Assert.Equal(w, back);
    }
}
