using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.Testing;

namespace Serde.Test
{
    internal static class Config
    {
        public static ReferenceAssemblies LatestTfRefs =>
			new ReferenceAssemblies (
				"net8.0",
				new PackageIdentity ("Microsoft.NETCore.App.Ref", "8.0.6"),
				Path.Combine ("ref", "net8.0"))
			.WithNuGetConfigFilePath (Path.Combine (
                GetDirectoryPath(),
                "..",
                "..",
                "NuGet.config"));

        private static string GetDirectoryPath([CallerFilePath]string path = "") => Path.GetDirectoryName(path)!;
    }
}