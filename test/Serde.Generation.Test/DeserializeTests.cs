
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using VerifyXunit;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    [UsesVerify]
    public class DeserializeTests
    {
        [Fact]
        public Task MemberSkip()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(Skip = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task MemberSkipSerialize()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(SkipDeserialize = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task MemberSkipDeserialize()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(SkipSerialize = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifyDeserialize(src);
        }
        [Fact]
        public Task Rgb()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red, Green, Blue;
}";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task NullableRefField()
        {
            var src = @"
[Serde.GenerateDeserialize]
partial struct S
{
    public string? F;
}";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task DeserializeMissing()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
readonly partial record struct SetToNull
{
    public string Present { get; init; }
    public string? Missing { get; init; }
    [SerdeMemberOptions(ThrowIfMissing = true)]
    public string? ThrowMissing { get; init; }
} ";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task Array()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial class ArrayField
{
    public int[] IntArr = new[] { 1, 2, 3 };
}";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task EnumMember()
        {
            var src = @"
[Serde.GenerateDeserialize]
partial class C
{
    public ColorInt ColorInt;
    public ColorByte ColorByte;
    public ColorLong ColorLong;
    public ColorULong ColorULong;
}
public enum ColorInt { Red = 3, Green = 5, Blue = 7 }
public enum ColorByte : byte { Red = 3, Green = 5, Blue = 7 }
public enum ColorLong : long { Red = 3, Green = 5, Blue = 7 }
public enum ColorULong : ulong { Red = 3, Green = 5, Blue = 7 }
";

            return GeneratorTestUtils.VerifyMultiFile(src);
        }

        private static Task VerifyDeserialize(
            string src,
            [CallerMemberName] string caller = "")
            => VerifyGeneratedCode(src,
                nameof(DeserializeTests),
                caller,
                multiFile: false);
    }
}