using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class FieldInitializerTests
    {
        [Fact]
        public Task FieldInitializers()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial class C
{
    // Compile-time constants
    public string Str = "hello";
    public int Num = 42;
    public bool Flag = true;
    public string? Nullable = null;
    public double Dbl = 3.14;
    public char Ch = 'x';

    // Static member access
    public string FromMethod = string.Empty;
    public int MaxInt = int.MaxValue;
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// Enum field initializers should be preserved as compile-time constants.
        /// </summary>
        [Fact]
        public Task FieldInitializerEnum()
        {
            var src = """
using Serde;

[GenerateDeserialize]
enum Color { Red, Green, Blue }

[GenerateDeserialize]
partial class C
{
    public Color Color = Color.Green;
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// Arithmetic/complex expressions in initializers should NOT be preserved
        /// (not compile-time constants from GetConstantValue's perspective when involving
        /// non-literal sub-expressions), falling back to default!.
        /// </summary>
        [Fact]
        public Task FieldInitializerUnsafe()
        {
            var src = """
using Serde;
using System.Collections.Generic;

[GenerateDeserialize]
partial class C
{
    // new expressions are not constants or static member accesses
    public List<int> Items = new List<int>();
    // Array creation is not a constant
    public int[] Arr = new int[10];
    // nameof references a symbol that may not be in scope in generated code
    public string Name = nameof(C);
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// An empty collection expression has no identifiers and is target-typed by the
        /// generated local, so it is preserved. Non-empty collection expressions may reference
        /// symbols that aren't in scope in generated code, so they fall back to default!.
        /// </summary>
        [Fact]
        public Task FieldInitializerEmptyCollection()
        {
            var src = """
using Serde;
using System.Collections.Generic;

[GenerateDeserialize]
partial class C
{
    public List<int> Items = [];
    public int[] Arr = [];
    public string[] Strs { get; set; } = [];
    // Non-empty collection expressions are not preserved
    public int[] NonEmpty = [1, 2];
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// Verify that null! on a non-nullable reference type roundtrips correctly.
        /// GetConstantValue returns null (without the !), so we need to re-add the
        /// null-forgiving operator for the generated code to compile under #nullable enable.
        /// </summary>
        [Fact]
        public Task FieldInitializerNullForgiving()
        {
            var src = """
using Serde;

[GenerateDeserialize]
partial class C
{
    // null! on non-nullable types — must roundtrip as null!
    public string S = null!;
    public int[] Arr = null!;
    // null on nullable type — no ! needed
    public string? N = null;
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// Primary constructor parameters referenced in field initializers must NOT be
        /// preserved. This was the root cause of https://github.com/serdedotnet/serde/issues/313.
        /// We use a record here so the primary ctor params are proper public properties
        /// The field `Extra = X + 1` references ctor param `X`, so GetConstantValue
        /// won't succeed and it falls back to default!.
        /// </summary>
        [Fact]
        public Task FieldInitializerPrimaryCtorParam()
        {
            var src = """
using Serde;

[GenerateDeserialize]
partial record C(int X)
{
    // References primary ctor parameter — must fall back to default!
    public int Extra = X + 1;
    // Safe constant — should be preserved
    public int Z = 100;
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// Enum constants that are not a direct member access (e.g. a cast like (Color)0)
        /// reach the constant path. The initializer text uses a simple type name that may
        /// rely on a using directive not present in the generated file, so it must be
        /// reconstructed with the fully-qualified enum type name.
        /// </summary>
        [Fact]
        public Task FieldInitializerEnumCast()
        {
            var src = """
using Serde;
using Other;

namespace Mine
{
    [GenerateDeserialize]
    partial class C
    {
        public Color Field = (Color)0;
        public int Y;
    }
}

namespace Other
{
    [GenerateDeserialize]
    public enum Color { Red, Green }
}
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// Field initializers are never preserved for an empty ForType proxy, even a constant
        /// that could be copied. The target is usually in another assembly, where initializers
        /// aren't visible, so ignoring them everywhere keeps the same members required no matter
        /// where the target lives. Every member falls back to default! and stays required.
        /// </summary>
        [Fact]
        public Task FieldInitializerForeignTypeProxy()
        {
            var src = """
using Serde;

class Target
{
    private static readonly int s_def = 7;
    public int X = s_def;
    public int Y;
    public int Z = 5;
}

[GenerateDeserialize(ForType = typeof(Target))]
partial struct TargetProxy { }
""";
            return VerifyDeserialize(src);
        }

        /// <summary>
        /// A union case's deserializer is generated in a proxy nested in the union, so its
        /// initializers are preserved if they are accessible from the union. A private static
        /// member of the case is not, so it falls back to default!, but a private const is
        /// still copied by value.
        /// </summary>
        [Fact]
        public Task FieldInitializerUnionCase()
        {
            var src = """
using Serde;

[GenerateDeserialize]
abstract partial record Shape
{
    private Shape() { }

    public sealed partial record Install : Shape
    {
        private const int PrivateConst = 3;
        private static readonly int s_private = 7;
        internal static readonly int Internal = 9;

        public bool Flag { get; init; } = true;
        public int FromPrivateConst { get; init; } = PrivateConst;
        public int FromPrivate { get; init; } = s_private;
        public int FromInternal { get; init; } = Internal;
    }
}
""";
            return VerifyDeserialize(src);
        }

        private static Task VerifyDeserialize(string src, [CallerMemberName] string caller = "") =>
            VerifyGeneratedCode(src, nameof(FieldInitializerTests), caller, multiFile: false);
    }
}
