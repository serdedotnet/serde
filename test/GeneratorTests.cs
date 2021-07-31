
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Serde.Test
{
    public class GeneratorTests
    {
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
            return VerifyGeneratedCode(src, "Rgb", @"
using Serde;

partial struct Rgb : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""Rgb"", 3);
        type.SerializeField(""Red"", new ByteWrap(Red));
        type.SerializeField(""Green"", new ByteWrap(Green));
        type.SerializeField(""Blue"", new ByteWrap(Blue));
        type.End();
    }
}");
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
            return VerifyGeneratedCode(src,
                // /0/Test0.cs(6,15): error ERR_DoesntImplementISerializable: The member 'S1.X's return type 'S2' doesn't implement Serde.ISerializable. Either implement the interface, or use a remote implementation.
                DiagnosticResult.CompilerError("ERR_DoesntImplementISerializable").WithSpan(6, 15, 6, 16));
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
            return VerifyGeneratedCode(src,
// /0/Test0.cs(4,8): error ERR_TypeNotPartial: The type 'S' has the `[GenerateSerializeAttribute]` applied, but the implementation can't be generated unless the type is marked 'partial'.
DiagnosticResult.CompilerError("ERR_TypeNotPartial").WithSpan(4, 8, 4, 9),
// /0/Test0.cs(6,7): error ERR_TypeNotPartial: The type 'C' has the `[GenerateSerializeAttribute]` applied, but the implementation can't be generated unless the type is marked 'partial'.
DiagnosticResult.CompilerError("ERR_TypeNotPartial").WithSpan(6, 7, 6, 8)
            );
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
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""IntArr"", new ArrayWrap<int, Int32Wrap>(IntArr));
        type.End();
    }
}");
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
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""NestedArr"", new ArrayWrap<int[], ArrayWrap<int, Int32Wrap>>(NestedArr));
        type.End();
    }
}");
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
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""NestedArr"", new ArrayWrap<int[], ArrayWrap<int, Int32Wrap>>(NestedArr));
        type.End();
    }
}");
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

            return VerifyGeneratedCode(src, new[] {
                ("TestCase15.Class0", @"
using Serde;

partial class TestCase15
{
    public partial class Class0 : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType(""Class0"", 2);
            type.SerializeField(""Field0"", new ArrayWrap<TestCase15.Class1>(Field0));
            type.SerializeField(""Field1"", new ArrayWrap<bool, BoolWrap>(Field1));
            type.End();
        }
    }
}"),
                ("TestCase15.Class1", @"
using Serde;

partial class TestCase15
{
    public partial class Class1 : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType(""Class1"", 2);
            type.SerializeField(""Field0"", new Int32Wrap(Field0));
            type.SerializeField(""Field1"", new ByteWrap(Field1));
            type.End();
        }
    }
}")
            });
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
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""Map"", new DictWrap<string, StringWrap, int, Int32Wrap>(Map));
        type.End();
    }
}");
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
            return VerifyGeneratedCode(src, new[] {
                ("C", @"
using Serde;

partial record C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""X"", new Int32Wrap(X));
        type.End();
    }
}"),
                ("C2", @"
using Serde;

partial class C2 : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C2"", 1);
        type.SerializeField(""Map"", new DictWrap<string, StringWrap, C, IdWrap<C>>(Map));
        type.End();
    }
}")
});
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
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""RDictionary"", new IDictWrap<string, StringWrap, int, Int32Wrap>(RDictionary));
        type.End();
    }
}");
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
        serializer.Serialize(_s.X);
        serializer.Serialize(_s.Y);
    }
}
[GenerateSerialize]
partial class C
{
    [SerdeWrap(typeof(SWrap))]
    public S S = new S();
}";
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""S"", new SWrap(S));
        type.End();
    }
}");
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
public struct SWrap<T, TWrap> : ISerialize, IWrap<S<T>, SWrap<T, TWrap>>
    where TWrap : struct, ISerialize, IWrap<T, TWrap>
{
    public SWrap<T, TWrap> Create(S<T> t) => new SWrap<T, TWrap>(t);
    private readonly S<T> _s;
    public SWrap(S<T> s)
    {
        _s = s;
    }
    void ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 1);
        var d = default(TWrap);
        type.SerializeField(""S"", d.Create(_s.Field));
        type.End();
    }
}
[GenerateSerialize]
partial class C
{
    [SerdeWrap(typeof(SWrap<,>))]
    public S<int> S = new S<int>(5);
}";
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""S"", new SWrap<int, Int32Wrap>(S));
        type.End();
    }
}");
        }
        
        internal static Task VerifyGeneratedCode(
            string src,
            params DiagnosticResult[] diagnostics)
            => VerifyGeneratedCode(src, Array.Empty<(string, string)>(), diagnostics);

        internal static Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => VerifyGeneratedCode(src, new[] { (typeName, expected)}, diagnostics);

        internal static Task VerifyGeneratedCode(
            string src,
            (string typeName, string expected)[] generated,
            params DiagnosticResult[] diagnostics)
        {
            var verifier = CreateVerifier(src);
            verifier.ExpectedDiagnostics.AddRange(diagnostics);
            foreach (var (typeName, expected) in generated)
            {
                verifier.TestState.GeneratedSources.Add((
                    Path.Combine("SerdeGenerator", $"Serde.{nameof(SerializeGenerator)}", $"{typeName}.ISerialize.cs"),
                    SourceText.From(expected, Encoding.UTF8))
                );
            }
            return verifier.RunAsync();
        }

        private static CSharpSourceGeneratorTest<SerializeGenerator, XUnitVerifier> CreateVerifier(string src)
        {
            var verifier = new CSharpSourceGeneratorTest<SerializeGenerator, XUnitVerifier>()
            {
                TestCode = src,
                ReferenceAssemblies = Config.LatestTfRefs,
            };
            verifier.TestState.AdditionalReferences.Add(typeof(Serde.GenerateSerializeAttribute).Assembly);
            return verifier;
        }
    }
}