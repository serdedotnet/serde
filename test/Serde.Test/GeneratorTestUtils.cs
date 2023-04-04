
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
using Microsoft.CodeAnalysis.Diagnostics;
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
            bool success = true;
            for (int i = 0; i < Math.Min(resultDiags.Length, diagnostics.Length); i++)
            {
                var expected = diagnostics[i];
                var actual = resultDiags[i];
                success &= DiagEquals(expected, actual);
            }
            success &= resultDiags.Length == diagnostics.Length;
            Assert.True(success,
                $"""
Expected:
{string.Join(Environment.NewLine, diagnostics.Select(d => d.ToString()))}
Actual:
{FormatDiagnostics("Program.cs", resultDiags.ToArray())}
""");
            for (int i = 0; i < Math.Min(generated.Length, result.GeneratedTrees.Length); i++)
            {
                var expected = generated[i];
                var actual = result.GeneratedTrees[i];
                Assert.Equal(expected.FileName, Path.GetFileNameWithoutExtension(actual.FilePath));
                Assert.Equal(expected.Source, actual.GetText().ToString());
            }
            Assert.Equal(generated.Length, result.GeneratedTrees.Length);
        }

        public static DiagnosticResult AsResult(this Diagnostic d)
            => new DiagnosticResult(d.Descriptor)
                .WithLocation(d.Location.GetLineSpan().Path, d.Location.GetLineSpan().StartLinePosition);

        public static string FormatDiagnostics(string defaultFilePath, params Diagnostic[] diagnostics)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < diagnostics.Length; ++i)
            {
                var diagnosticsId = diagnostics[i].Id;
                var location = diagnostics[i].Location;

                builder.Append("// ").AppendLine(diagnostics[i].ToString());

                DiagnosticAnalyzer? applicableAnalyzer = null;
                if (applicableAnalyzer != null)
                {
                    var analyzerType = applicableAnalyzer.GetType();
                    var rule = location != Location.None && location.IsInSource && applicableAnalyzer.SupportedDiagnostics.Length == 1 ? string.Empty : $"{analyzerType.Name}.{diagnosticsId}";

                    if (location == Location.None || !location.IsInSource)
                    {
                        builder.Append($"new DiagnosticResult({rule})");
                    }
                    else
                    {
                        var resultMethodName = location.SourceTree.FilePath.EndsWith(".cs") ? "VerifyCS.Diagnostic" : "VerifyVB.Diagnostic";
                        builder.Append($"{resultMethodName}({rule})");
                    }
                }
                else
                {
                    builder.Append(
                        diagnostics[i].Severity switch
                        {
                            DiagnosticSeverity.Error => $"{nameof(DiagnosticResult)}.{nameof(DiagnosticResult.CompilerError)}(\"{diagnostics[i].Id}\")",
                            DiagnosticSeverity.Warning => $"{nameof(DiagnosticResult)}.{nameof(DiagnosticResult.CompilerWarning)}(\"{diagnostics[i].Id}\")",
                            var severity => $"new {nameof(DiagnosticResult)}(\"{diagnostics[i].Id}\", {nameof(DiagnosticSeverity)}.{severity})",
                        });
                }

                if (location == Location.None)
                {
                    // No additional location data needed
                }
                else
                {
                    AppendLocation(diagnostics[i].Location);
                    foreach (var additionalLocation in diagnostics[i].AdditionalLocations)
                    {
                        AppendLocation(additionalLocation);
                    }
                }

                if (diagnostics[i].IsSuppressed)
                {
                    builder.Append($".{nameof(DiagnosticResult.WithIsSuppressed)}(true)");
                }

                builder.AppendLine(",");
            }

            return builder.ToString();

            // Local functions
            void AppendLocation(Location location)
            {
                var lineSpan = location.GetLineSpan();
                var pathString = location.IsInSource && lineSpan.Path == defaultFilePath ? string.Empty : $"\"{lineSpan.Path}\", ";
                var linePosition = lineSpan.StartLinePosition;
                var endLinePosition = lineSpan.EndLinePosition;
                builder.Append($".WithSpan({pathString}{linePosition.Line + 1}, {linePosition.Character + 1}, {endLinePosition.Line + 1}, {endLinePosition.Character + 1})");
            }
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