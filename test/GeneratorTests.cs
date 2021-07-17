
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
[GenerateISerialize]
partial struct Rgb
{
    public byte Red, Green, Blue;
}";
            return VerifyGeneratedCode(src, "Rgb", @"
using Serde;

partial struct Rgb : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
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
[GenerateISerialize]
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
[GenerateISerialize]
struct S { }
[GenerateISerialize]
class C { }";
            return VerifyGeneratedCode(src,
// /0/Test0.cs(4,8): error ERR_TypeNotPartial: The type 'S' has the `[GenerateISerializeAttribute]` applied, but the implementation can't be generated unless the type is marked 'partial'.
DiagnosticResult.CompilerError("ERR_TypeNotPartial").WithSpan(4, 8, 4, 9),
// /0/Test0.cs(6,7): error ERR_TypeNotPartial: The type 'C' has the `[GenerateISerializeAttribute]` applied, but the implementation can't be generated unless the type is marked 'partial'.
DiagnosticResult.CompilerError("ERR_TypeNotPartial").WithSpan(6, 7, 6, 8)
            );
        }

        [Fact]
        public Task TypeWithArray()
        {
            var src = @"
using Serde;
[GenerateISerialize]
partial class C
{
    public readonly int[] IntArr = new[] { 1, 2, 3 };
}";
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
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
[GenerateISerialize]
partial class C
{
    public readonly int[][] NestedArr = new[] { new[] { 1 }, new[] { 2 } };
}";
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
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
[GenerateISerialize]
partial class C
{
    public readonly int[][] NestedArr = new int[][] { };
}";
            return VerifyGeneratedCode(src, "C", @"
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""NestedArr"", new ArrayWrap<int[], ArrayWrap<int, Int32Wrap>>(NestedArr));
        type.End();
    }
}");
        }

        [Fact]
        public Task ArrayOfGenerateISerialize()
        {
            var src = @"
using Serde;

partial class TestCase15
{
    [GenerateISerialize]
    public partial class Class0
    {
        public Class1[] Field0 = new Class1[]{new Class1()};
        public bool[] Field1 = new bool[]{false};
    }

    [GenerateISerialize]
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
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
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
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
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
        public Task CustomWrapper()
        {
            var src = @"
using Serde;

[GenerateISerialize]
partial struct S
{
    public int X;
}

[GenerateISerialize]
partial class C
{
    [Serde(Wrapper = typeof(MemoryWrap<>))]
    public System.Memory<S> M;
}

partial readonly struct MemoryWrap<T> : ISerialize
{
    private System.Memory<T> _m;
    public MemoryWrap(System.Memory<T> m)
    {
        _m = m;
    }

    void ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable>(ref TSerializer serializer)
    {
        var e = serializer.SerializeEnumerable(_m.Length);
        foreach (var item in _m.Span)
        {
            e.SerializeElement(item);
        }
        e.End();
    }
}";
            return VerifyGeneratedCode(src, new[] { ("S", ""), ("C", "") });
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
            verifier.TestState.AdditionalReferences.Add(typeof(Serde.GenerateISerialize).Assembly);
            return verifier;
        }
    }
}