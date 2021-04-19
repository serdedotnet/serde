
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
            var allInOnePath = Path.Combine(Path.GetDirectoryName(curPath), "AllInOneSrc.cs");

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
            var type = serializer.SerializeType(""AllInOne"", 9);
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

        private Task VerifyGeneratedCode(string src, string typeName, string expected)
        {
            var verifier = new CSharpSourceGeneratorTest<SerdeGenerator, XUnitVerifier>()
            {
                TestCode = src,
                ReferenceAssemblies = Config.LatestTfRefs,
            };
            verifier.CompilerDiagnostics = CompilerDiagnostics.Warnings;
            verifier.TestState.AdditionalReferences.Add(typeof(Serde.GenerateSerdeAttribute).Assembly);
            verifier.TestState.GeneratedSources.Add((
                Path.Combine("SerdeGenerator", "Serde.SerdeGenerator", $"{typeName}.ISerialize.cs"),
                SourceText.From(expected, Encoding.UTF8))
            );
            return verifier.RunAsync();
        }
    }
}