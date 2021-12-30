
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace Serde.Test
{
    public class GeneratorDeserializeTests
    {
        [Fact]
        public Task Rgb()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red, Green, Blue;
}";
            return VerifyGeneratedCode(src, "Rgb", @"
#nullable enable
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""Red"", ""Green"", ""Blue""};
        return deserializer.DeserializeType<Rgb, SerdeVisitor>(""Rgb"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => ""Rgb"";
        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> red = default;
            Serde.Option<byte> green = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""Red"":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""Green"":
                        green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""Blue"":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type Rgb: '"" + key + ""'"");
                }
            }

            Rgb newType = new Rgb()
            {Red = red.Value, Green = green.Value, Blue = blue.Value, };
            return newType;
        }
    }
}");
        }

        [Fact]
        public Task Array()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial struct ArrayField
{
    public int[] IntArr = new[] { 1, 2, 3 };
}";
            return VerifyGeneratedCode(src, "ArrayField", @"
#nullable enable
using Serde;

partial struct ArrayField : Serde.IDeserialize<ArrayField>
{
    static ArrayField Serde.IDeserialize<ArrayField>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""IntArr""};
        return deserializer.DeserializeType<ArrayField, SerdeVisitor>(""ArrayField"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ArrayField>
    {
        public string ExpectedTypeName => ""ArrayField"";
        ArrayField Serde.IDeserializeVisitor<ArrayField>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int[]> intarr = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""IntArr"":
                        intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type ArrayField: '"" + key + ""'"");
                }
            }

            ArrayField newType = new ArrayField()
            {IntArr = intarr.Value, };
            return newType;
        }
    }
}");
        }

        private static Task VerifyGeneratedCode(
            string src,
            params DiagnosticResult[] diagnostics)
            => GeneratorSerializeTests.VerifyGeneratedCode(src, System.Array.Empty<(string, string)>(), diagnostics);

        private static Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => GeneratorSerializeTests.VerifyGeneratedCode(src, new[] { (typeName + ".IDeserialize", expected)}, diagnostics);
    }
}