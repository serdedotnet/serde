using System.Threading.Tasks;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test;

public class SerdeTests
{
    [Fact]
    public Task CommandResponseTest()
    {
        var src = """
using Serde;
using System.Collections.Generic;

[GenerateSerde]
public partial class CommandResponse<TResult, TProxy>
    where TResult : class
    where TProxy : ISerializeProvider<TResult>, IDeserializeProvider<TResult>
{
    public int Status { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<ArgumentInfo>? Arguments { get; set; }

    [SerdeMemberOptions(TypeParameterProxy = nameof(TProxy))]
    public TResult? Results { get; set; }

    public long Duration { get; set; }
}

[GenerateSerde]
public partial class ArgumentInfo
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
""";
        return VerifyMultiFile(src);
    }

    [Fact]
    public Task InvalidTypeParameterProxyTest()
    {
        var src = """
using Serde;

[GenerateSerde]
public partial class InvalidProxyTest<T>
{
    [SerdeMemberOptions(TypeParameterProxy = "NonExistentProxy")]
    public T? Value1 { get; set; }

    [SerdeMemberOptions(SerializeTypeParameterProxy = "NonExistentProxy")]
    public T? Value2 { get; set; }

    [SerdeMemberOptions(DeserializeTypeParameterProxy = "NonExistentProxy")]
    public T? Value3 { get; set; }
}
""";
        return VerifyDiagnostics(src);
    }
}