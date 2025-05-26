using System.Threading.Tasks;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test;

public class SerdeTests
{
    [Fact]
    public Task CustomSerdeObj()
    {
        var src = """
using System;
using Serde;
using Serde.Json;

[GenerateSerde(With = typeof(ColorSerdeObj))]
partial record Color(int R, int G, int B);

// Create a serde object for the Color type that serializes as a hex string
class ColorSerdeObj : ISerde<Color>
{
    // Color is serialized as a hex string, so it looks just like a string in the serialized form.
    public ISerdeInfo SerdeInfo => StringProxy.SerdeInfo;

    public void Serialize(Color color, ISerializer serializer)
    {
        var hex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        serializer.WriteString(hex);
    }

    public Color Deserialize(IDeserializer deserializer)
    {
        var hex = deserializer.ReadString();
        if (hex.Length != 7 || hex[0] != '#')
            throw new FormatException("Invalid hex color format");

        return new Color(
            Convert.ToInt32(hex[1..3], 16),
            Convert.ToInt32(hex[3..5], 16),
            Convert.ToInt32(hex[5..7], 16));
    }
}
static class CustomSerializationSample
{
    public static void Run()
    {
        var color = new Color(255, 0, 0);
        var json = JsonSerializer.Serialize(color);
        var deserializedColor = JsonSerializer.Deserialize<Color>(json);
    }
}
""";
        return VerifyMultiFile(src);
    }

    [Fact]
    public Task NestedNullable()
    {
        var src = """
using Serde;

[GenerateSerde]
public partial record SimpleRecord(int Id, string Name);

[GenerateSerde]
public partial record ComplexRecord(
    int Id,
    string? Description,
    SimpleRecord? NestedRecord);
""";
        return VerifyMultiFile(src);
    }

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
    public required TResult Results { get; set; }

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