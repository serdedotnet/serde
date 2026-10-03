using Serde.Json;
using Xunit;

namespace Serde.Test;

public partial class SerializeNullTests
{
    [GenerateSerde]
    private partial class MemberOption
    {
        [SerdeMemberOptions(SerializeNull = true)]
        public string? Value { get; set; }
    }

    [GenerateSerde]
    [SerdeTypeOptions(SerializeNull = true)]
    private partial class TypeOption
    {
        public string? Value { get; set; }
    }

    [Fact]
    public void NullStringEmittedWithMemberOption()
    {
        Assert.Equal("""{"value":null}""", JsonSerializer.Serialize(new MemberOption()));
        Assert.Equal(
            """{"value":"a"}""",
            JsonSerializer.Serialize(new MemberOption { Value = "a" })
        );
        Assert.Null(JsonSerializer.Deserialize<MemberOption>("""{"value":null}""").Value);
    }

    [Fact]
    public void NullStringEmittedWithTypeOption()
    {
        Assert.Equal("""{"value":null}""", JsonSerializer.Serialize(new TypeOption()));
        Assert.Equal("""{"value":"a"}""", JsonSerializer.Serialize(new TypeOption { Value = "a" }));
        Assert.Null(JsonSerializer.Deserialize<TypeOption>("""{"value":null}""").Value);
    }
}
