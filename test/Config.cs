
using System;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;

namespace Serde.Test
{
    internal static class Config
    {
        public static ReferenceAssemblies LatestTfRefs =>
			new ReferenceAssemblies (
				"net6.0",
				new PackageIdentity ("Microsoft.NETCore.App.Ref", "6.0.0-preview.7.21368.2"),
				Path.Combine ("ref", "net6.0"))
			.WithNuGetConfigFilePath (Path.Combine (
                GetDirectoryPath(),
                "..",
                "NuGet.config"));

        private static string GetDirectoryPath([CallerFilePath]string path = "") => Path.GetDirectoryName(path)!;
    }
}