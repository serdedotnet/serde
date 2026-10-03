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
}
