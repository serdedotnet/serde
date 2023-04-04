
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Serde.Test
{
    public static class GeneratorTestUtils
    {
        public static Task VerifyDiagnostics(
            string src,
            params DiagnosticResult[] diagnostics)
            => VerifyGeneratedCode(src, System.Array.Empty<(string, string)>(), diagnostics);

        public static Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => VerifyGeneratedCode(src, new[] { (typeName, expected)}, diagnostics);


        public static async Task VerifyGeneratedCode(
            string src,
            (string FileName, string Source)[] generated,
            params DiagnosticResult[] diagnostics)
        {
            var generatorInstance = new Generator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generatorInstance);
            var comp = await CreateCompilation(src);
            driver = driver.RunGeneratorsAndUpdateCompilation(comp, out var newComp, out _);
            var result = driver.GetRunResult();
            var compDiags = newComp.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error);
            var resultDiags = compDiags.Concat(result.Diagnostics).ToImmutableArray();
            for (int i = 0; i < Math.Min(resultDiags.Length, diagnostics.Length); i++)
            {
                var expected = diagnostics[i];
                var actual = resultDiags[i];
                Assert.True(DiagEquals(expected, actual), $"Expected {expected} but got {actual}");
            }
            Assert.True(diagnostics.Length == resultDiags.Length,
                $"Expected {string.Join(Environment.NewLine, diagnostics.Select(d => d.ToString()))} but got {string.Join(Environment.NewLine, resultDiags.Select(d => d.ToString()))}");
            for (int i = 0; i < Math.Min(generated.Length, result.GeneratedTrees.Length); i++)
            {
                var expected = generated[i];
                var actual = result.GeneratedTrees[i];
                Assert.Equal(expected.FileName, Path.GetFileNameWithoutExtension(actual.FilePath));
                Assert.Equal(expected.Source, actual.GetText().ToString());
            }
            Assert.Equal(generated.Length, result.GeneratedTrees.Length);
        }

        public static bool DiagEquals(DiagnosticResult expected, Diagnostic actual)
            => expected.Id == actual.Id
            && expected.Severity == actual.Severity
            && expected.Spans[0].Span == actual.Location.GetLineSpan();

        public static async Task<CSharpCompilation> CreateCompilation(string src)
        {
            IEnumerable<MetadataReference> refs = await Config.LatestTfRefs.ResolveAsync(null, default);
            refs = refs.Append(MetadataReference.CreateFromFile(typeof(Serde.GenerateSerialize).Assembly.Location));
            return CSharpCompilation.Create(
                Guid.NewGuid().ToString(),
                new[] { CSharpSyntaxTree.ParseText(src) },
                references: refs,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }
    }
}