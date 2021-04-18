
using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
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
        public Task AllInOne()
        {
            var curPath = GetPath();
            var allInOnePath = Path.Combine(Path.GetDirectoryName(curPath)!, "AllInOneSrc.cs");

            var src = File.ReadAllText(allInOnePath);
            // Add [GenerateSerde] to the class
            src = src.Replace("internal partial class AllInOne", @"[GenerateSerde] internal partial class AllInOne");
            var expected = @"
using Serde;

namespace Serde.Test
{
    internal partial class AllInOne : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType>(TSerializer serializer)
        {
            var type = serializer.SerializeType(""AllInOne"", 11);
            type.SerializeField(""BoolField"", new BoolWrap(BoolField));
            type.SerializeField(""CharField"", new CharWrap(CharField));
            type.SerializeField(""ByteField"", new ByteWrap(ByteField));
            type.SerializeField(""UShortField"", new UInt16Wrap(UShortField));
            type.SerializeField(""UIntField"", new UInt32Wrap(UIntField));
            type.SerializeField(""ULongField"", new UInt64Wrap(ULongField));
            type.SerializeField(""SByteField"", new SByteWrap(SByteField));
            type.SerializeField(""ShortField"", new Int16Wrap(ShortField));
            type.SerializeField(""IntField"", new Int32Wrap(IntField));
            type.SerializeField(""LongField"", new Int64Wrap(LongField));
            type.SerializeField(""StringField"", new StringWrap(StringField));
            type.End();
        }
    }
}";
            return VerifyGeneratedCode(src, "AllInOne", expected);

            static string GetPath([CallerFilePath] string path = "") => path;
        }

        [Fact]
        public Task TypeDoesntImplementISerialize()
        {
            var src = @"
using Serde;
[GenerateSerde]
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
[GenerateSerde]
struct S { }
[GenerateSerde]
class C { }";
            return VerifyGeneratedCode(src,
// /0/Test0.cs(4,8): error ERR_TypeNotPartial: The type 'S' has the `[GenerateSerdeAttribute]` applied, but the implementation can't be generated unless the type is marked 'partial'.
DiagnosticResult.CompilerError("ERR_TypeNotPartial").WithSpan(4, 8, 4, 9),
// /0/Test0.cs(6,7): error ERR_TypeNotPartial: The type 'C' has the `[GenerateSerdeAttribute]` applied, but the implementation can't be generated unless the type is marked 'partial'.
DiagnosticResult.CompilerError("ERR_TypeNotPartial").WithSpan(6, 7, 6, 8)
            );
        }

        private Task VerifyGeneratedCode(
            string src,
            params DiagnosticResult[] diagnostics)
        {
            var verifier = CreateVerifier(src);
            verifier.ExpectedDiagnostics.AddRange(diagnostics);
            return verifier.RunAsync();
        }

        private Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
        {
            var verifier = CreateVerifier(src);
            verifier.ExpectedDiagnostics.AddRange(diagnostics);
            verifier.TestState.GeneratedSources.Add((
                Path.Combine("SerdeGenerator", "Serde.SerdeGenerator", $"{typeName}.ISerialize.cs"),
                SourceText.From(expected, Encoding.UTF8))
            );
            return verifier.RunAsync();
        }

        private CSharpSourceGeneratorTest<SerdeGenerator, XUnitVerifier> CreateVerifier(string src)
        {
            var verifier = new CSharpSourceGeneratorTest<SerdeGenerator, XUnitVerifier>()
            {
                TestCode = src,
                ReferenceAssemblies = Config.LatestTfRefs,
            };
            verifier.TestState.AdditionalReferences.Add(typeof(Serde.GenerateSerdeAttribute).Assembly);
            return verifier;
        }
    }
}