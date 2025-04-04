using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class DeserializeTests
    {
        [Fact]
        public Task Inheritance()
        {
            var src = """
using Serde;

[GenerateDeserialize]
partial class A
{
    public int X;
}

[GenerateDeserialize]
partial class B : A
{
    public int Y;
}

[GenerateDeserialize]
partial class C : B
{
    public int Z;
}
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task MemberOverrides()
        {
            var src = """
using Serde;

[GenerateDeserialize]
partial record A(int X);

[GenerateDeserialize]
partial record B(int X, int Y) : A(X);
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task NestedExplicitDeserializeWrapper()
        {
            var src = """

using Serde;
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;

[GenerateDeserialize(ForType = typeof(BIND_OPTS))]
readonly partial record struct OptsWrap(BIND_OPTS Value);

[GenerateDeserialize]
partial struct S
{
    [SerdeMemberOptions(DeserializeProxy = typeof(ImmutableArrayProxy.De<BIND_OPTS, OptsWrap>))]
    public ImmutableArray<BIND_OPTS> Opts;
}

""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task DeserializeOnlyWrap()
        {
            var src = """
using Serde;
using System.Runtime.InteropServices.ComTypes;

[GenerateDeserialize(ForType = typeof(BIND_OPTS))]
readonly partial record struct Wrap(BIND_OPTS Value);

""";
            return VerifyDeserialize(src);
        }

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
        public Task MemberSkipDeserialize()
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
        public Task MemberSkipSerialize()
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
[Serde.GenerateDeserialize]
public enum ColorInt { Red = 3, Green = 5, Blue = 7 }
[Serde.GenerateDeserialize]
public enum ColorByte : byte { Red = 3, Green = 5, Blue = 7 }
[Serde.GenerateDeserialize]
public enum ColorLong : long { Red = 3, Green = 5, Blue = 7 }
[Serde.GenerateDeserialize]
public enum ColorULong : ulong { Red = 3, Green = 5, Blue = 7 }
";

            return GeneratorTestUtils.VerifyMultiFile(src);
        }

        [Fact]
        public Task NestedPartialClasses()
        {
            var src = """
using Serde;

partial class A
{
    private partial class B
    {
        private partial class C
        {
            [GenerateDeserialize]
            private partial class D
            {
                public int Field;
            }
        }
    }
}
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task NoParameterlessOrPrimaryCtor()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial class C
{
    public int A;
    public C(int A) { A = this.A; }
}
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task AbstractClass()
        {
            var src = """
using Serde;

[GenerateDeserialize]
abstract partial class Base { }
""";
            return GeneratorTestUtils.VerifyDiagnostics(src);
        }

        [Fact]
        public Task AbstractRecordPublicCtor()
        {
            var src = """
using Serde;

[GenerateDeserialize]
abstract partial record Base { }
""";
            return GeneratorTestUtils.VerifyDiagnostics(src);
        }

        [Fact]
        public Task EmptyDU()
        {
            var src = """
using Serde;

[GenerateDeserialize]
abstract partial record Base
{
    private Base() { }
}
""";
            return VerifyDeserialize(src);
        }

        [Fact]
        public Task BasicDU()
        {
            var src = """
using Serde;

namespace Some.Nested.Namespace;

[GenerateDeserialize]
abstract partial record Base
{
    private Base() { }

    public record A(int X) : Base { }
    public record B(string Y) : Base { }
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task BasicDUGenerateSerde()
        {
            var src = """
using Serde;

namespace Some.Nested.Namespace;

[GenerateSerde]
abstract partial record Base
{
    private Base() { }

    public record A(int X) : Base { }
    public record B(string Y) : Base { }
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task BasicDUGenerateSerializeAndDeserialize()
        {
            var src = """
using Serde;

namespace Some.Nested.Namespace;

[GenerateSerialize]
[GenerateDeserialize]
abstract partial record Base
{
    private Base() { }

    public record A(int X) : Base { }
    public record B(string Y) : Base { }
}
""";
            return VerifyMultiFile(src);
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