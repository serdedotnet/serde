using System;
using System.Collections.Generic;
using Serde.Json;
using Xunit;

namespace Serde.Test;

public partial class CaptureUnknownMembersTests
{
    [GenerateSerde]
    private partial class WithJsonUnknown
    {
        public required string Name { get; init; }
        public int? Age { get; init; }

        [SerdeMemberOptions(CaptureUnknownMembers = true)]
        public Dictionary<string, JsonValue>? Unknown { get; init; }
    }

    [GenerateSerde]
    private partial class WithStringUnknown
    {
        public required string Name { get; init; }

        [SerdeMemberOptions(CaptureUnknownMembers = true)]
        public Dictionary<string, string> Unknown { get; init; } = new();
    }

    [GenerateSerde]
    private partial record Outer(string Id, WithJsonUnknown Inner);

    [GenerateSerde]
    private partial class WithIntUnknown
    {
        public required string Name { get; init; }

        [SerdeMemberOptions(CaptureUnknownMembers = true)]
        public Dictionary<string, int>? Unknown { get; init; }
    }

    [GenerateSerde]
    [SerdeTypeOptions(AllowDuplicateKeys = true)]
    private partial class AllowDuplicates
    {
        public required string Name { get; init; }

        [SerdeMemberOptions(CaptureUnknownMembers = true)]
        public Dictionary<string, JsonValue>? Unknown { get; init; }
    }

    [GenerateSerde]
    private partial class WithSkipped
    {
        public required string Name { get; init; }

        [SerdeMemberOptions(SkipDeserialize = true)]
        public string Computed { get; init; } = "c";

        [SerdeMemberOptions(CaptureUnknownMembers = true)]
        public Dictionary<string, JsonValue>? Unknown { get; init; }
    }

    [GenerateSerde]
    private partial record RecordUnknown(
        string Name,
        [property: SerdeMemberOptions(CaptureUnknownMembers = true)]
            Dictionary<string, JsonValue>? Unknown
    );

    [GenerateSerde]
    private partial record RecordNonNullable(
        string Name,
        [property: SerdeMemberOptions(CaptureUnknownMembers = true)]
            Dictionary<string, JsonValue> Unknown
    );

    [Fact]
    public void CapturesUnknownMembers()
    {
        var json = """{ "name": "a", "x": 1, "y": [true, null], "age": 3, "z": { "w": "v" } }""";
        var result = JsonSerializer.Deserialize<WithJsonUnknown>(json);

        Assert.Equal("a", result.Name);
        Assert.Equal(3, result.Age);
        Assert.NotNull(result.Unknown);
        Assert.Equal(3, result.Unknown.Count);
        Assert.Equal(new JsonValue.Number(1), result.Unknown["x"]);
        Assert.Equal(
            new JsonValue.Array([new JsonValue.Bool(true), JsonValue.Null.Instance]),
            result.Unknown["y"]
        );
        Assert.Equal(new JsonValue.Object([("w", (JsonValue)"v")]), result.Unknown["z"]);
    }

    [Fact]
    public void NoUnknownMembersLeavesNull()
    {
        var result = JsonSerializer.Deserialize<WithJsonUnknown>("""{ "name": "a" }""");
        Assert.Null(result.Unknown);
        Assert.Null(result.Age);
    }

    [Fact]
    public void MissingRequiredMemberThrows()
    {
        var ex = Assert.Throws<UnassignedMemberException>(() =>
            JsonSerializer.Deserialize<WithJsonUnknown>("""{ "x": 1 }""")
        );
        Assert.Contains("name", ex.Message);
    }

    [Fact]
    public void Roundtrip()
    {
        var json = """{"name":"a","age":3,"x":1,"y":"s"}""";
        var result = JsonSerializer.Deserialize<WithJsonUnknown>(json);
        Assert.Equal(json, JsonSerializer.Serialize(result));
    }

    [Fact]
    public void EscapedNamesRoundtrip()
    {
        var json = """{"name":"a","quote\"back\\slash":1,"\u00fc":2,"tab\t":3}""";
        var result = JsonSerializer.Deserialize<WithJsonUnknown>(json);
        Assert.Equal(["quote\"back\\slash", "\u00fc", "tab\t"], result.Unknown!.Keys);

        var again = JsonSerializer.Deserialize<WithJsonUnknown>(JsonSerializer.Serialize(result));
        Assert.Equal(result.Unknown, again.Unknown);
    }

    [Fact]
    public void NullDeclaredMemberIsSkippedWithItsKey()
    {
        var value = new WithJsonUnknown
        {
            Name = "a",
            Unknown = new() { ["x"] = 1 },
        };
        Assert.Equal("""{"name":"a","x":1}""", JsonSerializer.Serialize(value));
    }

    [Fact]
    public void TypedUnknownValues()
    {
        var result = JsonSerializer.Deserialize<WithStringUnknown>(
            """{ "name": "a", "x": "1", "y": "2" }"""
        );
        Assert.Equal(new Dictionary<string, string> { ["x"] = "1", ["y"] = "2" }, result.Unknown);

        var empty = JsonSerializer.Deserialize<WithStringUnknown>("""{ "name": "a" }""");
        Assert.Empty(empty.Unknown);
    }

    [Fact]
    public void TypedUnknownValueMismatchThrows()
    {
        Assert.ThrowsAny<Exception>(() =>
            JsonSerializer.Deserialize<WithStringUnknown>("""{ "name": "a", "x": 1 }""")
        );
    }

    [Fact]
    public void NestedInStruct()
    {
        var json = """{"id":"o","inner":{"name":"a","q":false}}""";
        var result = JsonSerializer.Deserialize<Outer>(json);
        Assert.Equal(new JsonValue.Bool(false), result.Inner.Unknown!["q"]);
        Assert.Equal(json, JsonSerializer.Serialize(result));
    }

    [Fact]
    public void PrimaryConstructor()
    {
        var json = """{"name":"a","x":1}""";
        var result = JsonSerializer.Deserialize<RecordUnknown>(json);
        Assert.Equal(new JsonValue.Number(1), result.Unknown!["x"]);
        Assert.Equal(json, JsonSerializer.Serialize(result));
    }

    [Fact]
    public void NonNullablePrimaryConstructor()
    {
        var result = JsonSerializer.Deserialize<RecordNonNullable>("""{"name":"a"}""");
        Assert.NotNull(result.Unknown);
        Assert.Empty(result.Unknown);
    }

    [Fact]
    public void ValueTypeValues()
    {
        var json = """{"name":"a","x":1,"y":2}""";
        var result = JsonSerializer.Deserialize<WithIntUnknown>(json);
        Assert.Equal(new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 }, result.Unknown);
        Assert.Equal(json, JsonSerializer.Serialize(result));
    }

    [Fact]
    public void UnknownKeyCollidingWithDeclaredMemberThrows()
    {
        var value = new WithJsonUnknown
        {
            Name = "a",
            Unknown = new() { ["name"] = "b" },
        };
        Assert.Throws<InvalidOperationException>(() => JsonSerializer.Serialize(value));
    }

    [Fact]
    public void MemberIsNotAField()
    {
        var info = SerdeInfoProvider.GetSerializeInfo<WithJsonUnknown>();
        Assert.Equal(2, info.FieldCount);
        Assert.Equal("name", info.GetFieldStringName(0));
        Assert.Equal("age", info.GetFieldStringName(1));
    }

    [Fact]
    public void DuplicateUnknownKeyThrows()
    {
        var ex = Assert.Throws<DeserializeException>(() =>
            JsonSerializer.Deserialize<WithJsonUnknown>("""{ "name": "a", "x": 1, "x": 2 }""")
        );
        Assert.Contains("Duplicate key 'x'", ex.Message);
    }

    [Fact]
    public void DuplicateUnknownKeyAllowed()
    {
        var result = JsonSerializer.Deserialize<AllowDuplicates>(
            """{ "name": "a", "x": 1, "x": 2 }"""
        );
        Assert.Equal(new JsonValue.Number(2), result.Unknown!["x"]);
    }

    [Fact]
    public void DuplicateDeclaredMemberStillThrows()
    {
        var ex = Assert.Throws<DeserializeException>(() =>
            JsonSerializer.Deserialize<WithJsonUnknown>("""{ "x": 1, "name": "a", "name": "b" }""")
        );
        Assert.Contains("Duplicate key 'name'", ex.Message);
    }

    [Fact]
    public void SkippedFieldIsNotCaptured()
    {
        var result = JsonSerializer.Deserialize<WithSkipped>(
            """{ "name": "a", "computed": "z", "x": 1 }"""
        );
        Assert.Equal("c", result.Computed);
        Assert.Equal(["x"], result.Unknown!.Keys);
        Assert.Equal("""{"name":"a","computed":"c","x":1}""", JsonSerializer.Serialize(result));
    }
}
