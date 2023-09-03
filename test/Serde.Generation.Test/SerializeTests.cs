
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using VerifyXunit;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    [UsesVerify]
    public class SerializeTests
    {
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
    where T1 : ISerialize
    where T2 : ISerialize?
    where T3 : class, ISerialize
    where T4 : class?, ISerialize
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
    where TSerialize : struct, ISerialize
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
        public Task IDictionaryImplGenerate()
        {
            var src = @"
using System;
using System.Collections;
using System.Collections.Generic;
using Serde;

record R(Dictionary<string, int> D) : IDictionary<string, int>
{
    public int this[string key] { get => ((IDictionary<string, int>)D)[key]; set => ((IDictionary<string, int>)D)[key] = value; }

    public ICollection<string> Keys => ((IDictionary<string, int>)D).Keys;

    public ICollection<int> Values => ((IDictionary<string, int>)D).Values;

    public int Count => ((ICollection<KeyValuePair<string, int>>)D).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<string, int>>)D).IsReadOnly;

    public void Add(string key, int value)
    {
        ((IDictionary<string, int>)D).Add(key, value);
    }

    public void Add(KeyValuePair<string, int> item)
    {
        ((ICollection<KeyValuePair<string, int>>)D).Add(item);
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<string, int>>)D).Clear();
    }

    public bool Contains(KeyValuePair<string, int> item)
    {
        return ((ICollection<KeyValuePair<string, int>>)D).Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return ((IDictionary<string, int>)D).ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, int>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<string, int>>)D).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, int>>)D).GetEnumerator();
    }

    public bool Remove(string key)
    {
        return ((IDictionary<string, int>)D).Remove(key);
    }

    public bool Remove(KeyValuePair<string, int> item)
    {
        return ((ICollection<KeyValuePair<string, int>>)D).Remove(item);
    }

    public bool TryGetValue(string key, out int value)
    {
        return ((IDictionary<string, int>)D).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)D).GetEnumerator();
    }
}
[GenerateSerialize]
partial class C
{
    public R RDictionary;
}";
            return VerifySerialize(src);
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
public struct SWrap : ISerialize
{
    private readonly S _s;
    public SWrap(S s)
    {
        _s = s;
    }
    void ISerialize.Serialize(ISerializer serializer)
    {
        serializer.SerializeI32(_s.X);
        serializer.SerializeI32(_s.Y);
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
    public readonly struct SerializeImpl<T, TWrap> : ISerialize, ISerializeWrap<S<T>, SerializeImpl<T, TWrap>>
        where TWrap : struct, ISerialize, ISerializeWrap<T, TWrap>
    {
        public static SerializeImpl<T, TWrap> Create(S<T> t) => new(t);
        private readonly S<T> _s;
        public SerializeImpl(S<T> s)
        {
            _s = s;
        }
        void ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType(""S"", 1);
            type.SerializeField(""s"", TWrap.Create(_s.Field));
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
public readonly struct SWrap<T, TWrap> : ISerialize, ISerializeWrap<S<T>, SWrap<T, TWrap>>
    where TWrap : struct, ISerialize, ISerializeWrap<T, TWrap>
{
    public static SWrap<T, TWrap> Create(S<T> t) => new SWrap<T, TWrap>(t);
    private readonly S<T> _s;
    public SWrap(S<T> s)
    {
        _s = s;
    }
    void ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 1);
        type.SerializeField(""s"", TWrap.Create(_s.Field));
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

        private static Task VerifySerialize(
            string src,
            [CallerMemberName] string callerName = "")
            => VerifyGeneratedCode(src, nameof(SerializeTests), callerName, multiFile: false);
    }
}