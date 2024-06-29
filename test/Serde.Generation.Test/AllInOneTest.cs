using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Serde.Test;

public sealed class AllInOneTest
{
    [Fact]
    public Task GeneratorTest()
    {
        var curPath = GetPath();
        var allInOnePath = Path.Combine(Path.GetDirectoryName(curPath)!, "..", "Serde.Test", "AllInOneSrc.cs");

        var src = File.ReadAllText(allInOnePath);
        return GeneratorTestUtils.VerifyMultiFile(src);

        static string GetPath([CallerFilePath] string path = "") => path;
    }
}