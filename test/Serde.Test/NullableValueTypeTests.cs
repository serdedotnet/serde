using Serde.Json;
using Xunit;

namespace Serde.Test;

/// <summary>
/// Tests for JSON serialization/deserialization of nullable value-type members, covering both
/// serde null-handling behaviors (omit the field by default, or emit "field":null when
/// <see cref="SerdeMemberOptions.SerializeNull"/> is set) and the empty-nested-object parsing
/// edge case those behaviors can produce.
/// </summary>
public partial class NullableValueTypeTests
{
    [GenerateSerde]
    public partial record OmitByDefault
    {
        public int? Value { get; set; }
    }

    [GenerateSerde]
    public partial record EmitNull
    {
        [SerdeMemberOptions(SerializeNull = true)]
        public int? Value { get; set; }
    }

    [GenerateSerde]
    public partial record TwoFields
    {
        public int? First { get; set; }
        public int? Second { get; set; }
    }

    [GenerateSerde]
    public partial record Outer
    {
        public OmitByDefault Inner { get; set; } = new();
        public int Trailing { get; set; }
    }

    [Fact]
    public void NullField_OmittedByDefault()
    {
        Assert.Equal("{}", JsonSerializer.Serialize(new OmitByDefault { Value = null }));
        Assert.Equal("""{"value":5}""", JsonSerializer.Serialize(new OmitByDefault { Value = 5 }));
    }

    [Fact]
    public void NullField_EmittedWhenSerializeNull()
    {
        Assert.Equal("""{"value":null}""", JsonSerializer.Serialize(new EmitNull { Value = null }));
        Assert.Equal("""{"value":5}""", JsonSerializer.Serialize(new EmitNull { Value = 5 }));
    }

    [Fact]
    public void NullField_Roundtrips()
    {
        foreach (var value in new int?[] { null, 0, 42 })
        {
            Assert.Equal(
                value,
                JsonSerializer
                    .Deserialize<OmitByDefault>(
                        JsonSerializer.Serialize(new OmitByDefault { Value = value })
                    )
                    .Value
            );
            Assert.Equal(
                value,
                JsonSerializer
                    .Deserialize<EmitNull>(JsonSerializer.Serialize(new EmitNull { Value = value }))
                    .Value
            );
        }
    }

    [Fact]
    public void OmittedField_DeserializesToNull()
    {
        var result = JsonSerializer.Deserialize<TwoFields>("""{"second":7}""");
        Assert.Null(result.First);
        Assert.Equal(7, result.Second);
    }

    /// <summary>
    /// Regression: an empty nested object ("{}", produced when all of a nested type's nullable
    /// fields are null and omitted) followed by another field must not corrupt the enclosing
    /// object's parsing. The JSON deserializer shares a "first member" flag across nesting levels,
    /// and reading an empty object previously left it stale, producing a spurious
    /// "Expected property name" error on the following field.
    /// </summary>
    [Fact]
    public void EmptyNestedObject_FollowedByField_Roundtrips()
    {
        var json = JsonSerializer.Serialize(new Outer { Inner = new(), Trailing = 9 });
        Assert.Equal("""{"inner":{},"trailing":9}""", json);

        var result = JsonSerializer.Deserialize<Outer>(json);
        Assert.Null(result.Inner.Value);
        Assert.Equal(9, result.Trailing);
    }
}
