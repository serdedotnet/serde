
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using VerifyXunit;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class SerializeTests
    {
        [Fact]
        public Task NestedExplicitSerializeWrapper()
        {
            var src = """

using Serde;
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;

[GenerateSerde(ThroughMember = nameof(Value))]
readonly partial record struct OPTSWrap(BIND_OPTS Value);

[GenerateSerde]
partial struct S
{
    [SerdeMemberOptions(
        WrapperSerialize = typeof(ImmutableArrayWrap.SerializeImpl<BIND_OPTS, OPTSWrap>),
        WrapperDeserialize = typeof(ImmutableArrayWrap.DeserializeImpl<BIND_OPTS, OPTSWrap>))]
    public ImmutableArray<BIND_OPTS> Opts;
}

""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task SerializeOnlyWrapper()
        {
            var src = """
using Serde;
using System.Collections.Specialized;

[GenerateSerialize(ThroughMember = nameof(Value))]
readonly partial record struct SectionWrap(BitVector32.Section Value);

""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task MemberSkip()
        {
            var src = """
using Serde;
[GenerateSerialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(Skip = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifySerialize(src);
        }

        [Fact]
        public Task MemberSkipSerialize()
        {
            var src = """
using Serde;
[GenerateSerialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(SkipSerialize = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifySerialize(src);
        }

        [Fact]
        public Task MemberSkipDeserialize()
        {
            var src = """
using Serde;
[GenerateSerialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(SkipDeserialize = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifySerialize(src);
        }

        [Fact]
        public Task Rgb()
        {
            var src = @"
using Serde;
[GenerateSerialize]
partial struct Rgb
{
    public byte Red, Green, Blue;
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task NullableRefFields()
        {
            var src = @"
using System;
using Serde;

[GenerateSerialize]
partial struct S<T1, T2, T3, T4, T5>
    where T1 : ISerialize<T1>
    where T2 : ISerialize<T2>?
    where T3 : class, ISerialize<T3>
    where T4 : class?, ISerialize<T4>
{
    public string? FS;
    public T1 F1;
    public T2 F2;
    public T3? F3;
    public T4 F4;
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task NullableFields()
        {
            var src = """
using Serde;
[GenerateSerialize]
partial struct S<T1, T2, TSerialize>
    where T1 : int?
    where T2 : TSerialize?
    where TSerialize : struct, ISerialize<TSerialize>
{
    public int? FI;
    public T1 F1;
    public T2 F2;
    public TSerialize? F3;
}
""";
            return VerifySerialize(src);
        }

        [Fact]
        public Task TypeDoesntImplementISerialize()
        {
            var src = @"
using Serde;
[GenerateSerialize]
partial struct S1
{
    public S2 X;
}
struct S2 { }";
            return VerifySerialize(src);
        }

        [Fact]
        public Task TypeNotPartial()
        {
            var src = @"
using Serde;
[GenerateSerialize]
struct S { }
[GenerateSerialize]
class C { }";
            return VerifyDiagnostics(src);
        }

        [Fact]
        public Task TypeWithArray()
        {
            var src = @"
using Serde;
[GenerateSerialize]
partial class C
{
    public readonly int[] IntArr = new[] { 1, 2, 3 };
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task NestedArray()
        {
            var src = @"
using Serde;
[GenerateSerialize]
partial class C
{
    public readonly int[][] NestedArr = new[] { new[] { 1 }, new[] { 2 } };
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task NestedArray2()
        {
            var src = @"
using Serde;
[GenerateSerialize]
partial class C
{
    public readonly int[][] NestedArr = new int[][] { };
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task ArrayOfGenerateSerialize()
        {
            var src = @"
using Serde;

partial class TestCase15
{
    [GenerateSerialize]
    public partial class Class0
    {
        public Class1[] Field0 = new Class1[]{new Class1()};
        public bool[] Field1 = new bool[]{false};
    }

    [GenerateSerialize]
    public partial class Class1
    {
        public int Field0 = int.MaxValue;
        public byte Field1 = byte.MaxValue;
    }
}";

            return VerifyMultiFile(src);
        }

        [Fact]
        public Task DictionaryGenerate()
        {
            var src = @"
using Serde;
using System.Collections.Generic;

[GenerateSerialize]
partial class C
{
    public readonly Dictionary<string, int> Map = new Dictionary<string, int>()
    {
        [""abc""] = 5,
        [""def""] = 3
    };
}
";
            return VerifySerialize(src);
        }

        [Fact]
        public Task DictionaryGenerate2()
        {
            var src = @"
using Serde;
using System.Collections.Generic;

[GenerateSerialize]
partial record C(int X);

[GenerateSerialize]
partial class C2
{
    public readonly Dictionary<string, C> Map = new Dictionary<string, C>()
    {
        [""abc""] = new C(11),
        [""def""] = new C(3)
    };
}
";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task ExplicitWrapper()
        {
            var src = @"
using Serde;

public struct S
{
    public int X;
    public int Y;
    public S(int x, int y)
    {
        X = x;
        Y = y;
    }
}
public struct SWrap : ISerialize<S>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        ""S"",
        typeof(S).GetCustomAttributesData(),
        new (string, ISerdeInfo, System.Reflection.MemberInfo)[] {
            (""x"", SerdeInfoProvider.GetInfo<Int32Wrap>(), typeof(S).GetField(""X"")!),
            (""y"", SerdeInfoProvider.GetInfo<Int32Wrap>(), typeof(S).GetField(""Y"")!),
        });
    void ISerialize<S>.Serialize(S value, ISerializer serializer)
    {
        serializer.SerializeI32(value.X);
        serializer.SerializeI32(value.Y);
    }
}
[GenerateSerialize]
partial class C
{
    [SerdeWrap(typeof(SWrap))]
    public S S = new S();
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task ExplicitGenericWrapper()
        {
            var src = @"
using Serde;

public struct S<T>
{
    public T Field;
    public S(T f)
    {
        Field = f;
    }
}
public static class SWrap
{
    internal static class SWrapTypeInfo<T> where T : ISerdeInfoProvider
    {
        public static readonly ISerdeInfo TypeInfo = Serde.SerdeInfo.MakeCustom(
            ""S"",
            typeof(S<>).GetCustomAttributesData(),
            new (string, ISerdeInfo, System.Reflection.MemberInfo)[] {
                (""s"", Serde.SerdeInfoProvider.GetInfo<T>(), typeof(S<>).GetField(""Field"")!) });
    }
    public readonly struct SerializeImpl<T, TWrap> : ISerialize<S<T>>
        where TWrap : struct, ISerialize<T>
    {
        public static ISerdeInfo SerdeInfo => SWrapTypeInfo<TWrap>.TypeInfo;
        void ISerialize<S<T>>.Serialize(S<T> value, ISerializer serializer)
        {
            var _l_serdeInfo = SerdeInfo;
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<T, TWrap>(_l_serdeInfo, 0, value.Field);
            type.End();
        }
    }
}
[GenerateSerialize]
partial class C
{
    [SerdeWrap(typeof(SWrap))]
    public S<int> S = new S<int>(5);
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task WrongGenericWrapperForm()
        {
            var src = @"
using Serde;

public struct S<T>
{
    public T Field;
    public S(T f)
    {
        Field = f;
    }
}
public readonly struct SWrap<T, TWrap>
    : ISerialize<T>
    where TWrap : struct, ISerialize<T>
{
    public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        ""S"",
        typeof(S<>).GetCustomAttributesData(),
        new (string, ISerdeInfo, System.Reflection.MemberInfo)[] {
            (""s"", SerdeInfoProvider.GetInfo<TWrap>(), typeof(S<>).GetField(""Field"")!) });

    void ISerialize<T>.Serialize(T value, ISerializer serializer)
    {
        var _l_serdeInfo = SerdeInfo;
        var type = serializer.SerializeType(_l_serdeInfo);
        type.SerializeField<T, TWrap>(_l_serdeInfo, 0, value);
        type.End();
    }
}
[GenerateSerialize]
partial class C
{
    [SerdeWrap(typeof(SWrap<,>))]
    public S<int> S = new S<int>(5);
}";
            return VerifySerialize(src);
        }

        [Fact]
        public Task EnumMember()
        {
            var src = @"
namespace Some.Nested.Namespace;

[Serde.GenerateSerialize]
partial class C
{
    public ColorInt ColorInt;
    public ColorByte ColorByte;
    public ColorLong ColorLong;
    public ColorULong ColorULong;
}
[Serde.GenerateSerialize]
public enum ColorInt { Red = 3, Green = 5, Blue = 7 }
[Serde.GenerateSerialize]
public enum ColorByte : byte { Red = 3, Green = 5, Blue = 7 }
[Serde.GenerateSerialize]
public enum ColorLong : long { Red = 3, Green = 5, Blue = 7 }
[Serde.GenerateSerialize]
public enum ColorULong : ulong { Red = 3, Green = 5, Blue = 7 }
";

            return VerifyMultiFile(src);
        }

        [Fact]
        public Task NestedEnumWrapper()
        {
            var src = """
using Serde;

[GenerateSerialize]
partial class C
{
    public Rgb? ColorOpt;
}

[GenerateSerialize]
public enum Rgb { Red, Green, Blue }
""";
            return VerifyMultiFile(src);
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
            [GenerateSerialize]
            private partial class D
            {
                public int Field;
            }
        }
    }
}
""";
            return VerifySerialize(src);
        }

        private static Task VerifySerialize(
            string src,
            [CallerMemberName] string? callerName = null)
        {
            Assert.NotNull(callerName);
            return VerifyGeneratedCode(src, nameof(SerializeTests), callerName, multiFile: false);
        }
    }
}