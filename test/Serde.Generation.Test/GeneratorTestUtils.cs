
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Serde.Test
{
    public static class GeneratorTestUtils
    {
        public static Task VerifyDiagnostics(
            string src,
            params DiagnosticResult[] diagnostics)
            => VerifyMultiFile(src);

        public static Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => VerifyMultiFile(src);

        public static Task<VerifyResult[]> VerifyMultiFile(string src, MetadataReference[]? additionalRefs = null)
        {
            var settings = new VerifySettings();
            settings.UseDirectory("test_output/");
            settings.UseUniqueDirectory();
            return VerifyGeneratedCode(src, settings, additionalRefs);
        }

        public static Task<VerifyResult[]> VerifyGeneratedCode(
            string src,
            string directoryName,
            string testMethodName,
            bool multiFile)
        {
            var settings = new VerifySettings();
            settings.UseDirectory("test_output/" + directoryName);
            settings.UseFileName(testMethodName);
            if (multiFile)
            {
                settings.UseUniqueDirectory();
            }
            return VerifyGeneratedCode(src, settings);
        }

        public static async Task<VerifyResult[]> VerifyGeneratedCode(
            string src,
            VerifySettings settings,
            MetadataReference[]? additionalRefs = null)
        {
            var generatorInstance = new SerdeImplRoslynGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generatorInstance);
            Compilation comp = await CreateCompilation(src, additionalRefs);
            driver = driver.RunGeneratorsAndUpdateCompilation(comp, out comp, out _);
            var verify = Verifier.Verify(driver, settings);
            var diags = comp.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
            if (diags.Any())
            {
                return new[] { await verify.AppendContentAsFile(SerializeDiagnostics(diags), name: "FinalDiagnostics") };
            }
            else
            {
                return new[] { await verify };
            }
        }

        private static string SerializeDiagnostics(IEnumerable<Diagnostic> diags)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions {
                Indented = true,
                Encoder = (JavaScriptEncoder?)JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            writer.WriteStartArray();
            foreach (var diag in diags)
            {
                writer.WriteStartObject();
                writer.WriteString("Id", diag.Id);
                var descriptor = diag.Descriptor;
                writer.WriteString("Title", descriptor.Title.ToString());
                writer.WriteString("Severity", diag.Severity.ToString());
                writer.WriteString("WarningLevel", diag.WarningLevel.ToString());
                writer.WriteString("Location", diag.Location.GetMappedLineSpan().ToString());
                var description = descriptor.Description.ToString();
                if (!string.IsNullOrWhiteSpace(description))
                {
                    writer.WriteString("Description", description);
                }

                var help = descriptor.HelpLinkUri;
                if (!string.IsNullOrWhiteSpace(help))
                {
                    writer.WriteString("HelpLink", help);
                }

                writer.WriteString("MessageFormat", descriptor.MessageFormat.ToString());
                writer.WriteString("Message", diag.GetMessage());
                writer.WriteString("Category", descriptor.Category);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static async Task<CSharpCompilation> CreateCompilation(string src, MetadataReference[]? additionalRefs = null)
        {
            additionalRefs ??= Array.Empty<MetadataReference>();
            IEnumerable<MetadataReference> refs = await Config.LatestTfRefs.ResolveAsync(null, default);
            refs = refs.Concat(additionalRefs);
            refs = refs.Append(MetadataReference.CreateFromFile(typeof(Serde.GenerateSerialize).Assembly.Location));
            return CSharpCompilation.Create(
                Guid.NewGuid().ToString(),
                new[] { CSharpSyntaxTree.ParseText(src) },
                references: refs,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }
    }
}