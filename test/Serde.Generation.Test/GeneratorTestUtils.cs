
using System;
using System.Collections;
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

        private static readonly Lazy<VerifySettings> s_cachedSettings = new Lazy<VerifySettings>(() =>
        {
            var settings = new VerifySettings();
            settings.UseDirectory("test_output/");
            settings.UseUniqueDirectory();
            return settings;
        });

        public static Task<VerifyResult> VerifyMultiFile(string src)
        {
            return VerifyGeneratedCode(src, s_cachedSettings.Value);
        }

        public static Task<VerifyResult> VerifyGeneratedCode(
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

        private static async Task<VerifyResult> VerifyGeneratedCode(string src, VerifySettings settings)
        {
            var generatorInstance = new SerdeImplRoslynGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generatorInstance);
            var comp = await CreateCompilation(src);
            driver = driver.RunGenerators(comp);
            return await Verifier.Verify(driver, settings);
        }

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